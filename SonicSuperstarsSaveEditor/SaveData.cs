using System.IO.Compression;
using System.IO.Hashing;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SonicSuperstarsSaveEditor;
[Flags]
public enum PackedAttribute {
    None = 0,
    Crypto = 1,
    Compress = 2
}

internal class SaveData {
    public uint FormatVersion { get; set; }
    public uint BinaryVersion { get; set; }
    public uint Reserve { get; set; }
    public PackedAttribute Attribute { get; set; }
    [JsonConverter(typeof(ByteArrayConverter))]
    public byte[] PackedData { get; set; }

    public byte[] Unpack() {
        if (this.PackedData == null || this.PackedData.Length == 0) {
            Console.WriteLine("PackedData is not populated! This is not a valid save file");
            return Array.Empty<byte>();
        }
        if (FormatVersion != 3) {
            Console.WriteLine("Warning: FormatVersion is not 3. Untested version");
        }
        byte[] packedData = this.PackedData;
        if ((this.Attribute & PackedAttribute.Crypto) == PackedAttribute.Crypto) {
            try {
                using Aes aes = Aes.Create();
                aes.IV = new byte[16] { 120, 83, 106, 50, 111, 116, 51, 73, 46, 46, 46, 46, 46, 46, 46, 46 };
                aes.Key = new byte[32] { 90, 120, 116, 97, 48, 90, 121, 121, 117, 105, 89, 112, 79, 49, 97, 52, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46 };
                var decryptor = aes.CreateDecryptor();
                using MemoryStream msDecrypt = new();
                using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Write);
                csDecrypt.Write(packedData, 0, packedData.Length);
                csDecrypt.FlushFinalBlock();
                packedData = msDecrypt.ToArray();
            }
            catch (Exception ex) {
                Console.WriteLine($"Failed to decrypt data: {ex.Message}");
                return Array.Empty<byte>();
            }
            if (packedData.Length == 0) {
                Console.WriteLine($"Failed to decrypt data: Decrypted data is empty");
                return Array.Empty<byte>();
            }
        }
        if ((this.Attribute & PackedAttribute.Compress) == PackedAttribute.Compress) {
            try {
                using MemoryStream resultStream = new();
                using (MemoryStream ms = new(packedData)) {
                    using GZipStream gZipStream = new(ms, CompressionMode.Decompress);
                    gZipStream.CopyTo(resultStream);
                }
                packedData = resultStream.ToArray();
            }
            catch (Exception ex) {
                Console.WriteLine($"Failed to decompress data: {ex.Message}");
                return Array.Empty<byte>();
            }
        }
        try {
            Crc32 crc32 = new();
            crc32.Append(packedData);
            byte[] hashBytes = crc32.GetHashAndReset();
            uint hash = BitConverter.ToUInt32(hashBytes);
            if (hash != this.BinaryVersion) {
                Console.WriteLine("Warning: Checksum does not match. Data possibly corrupt");
            }
            return packedData;
        }
        catch (Exception ex) {
            Console.WriteLine($"Failed to deserialize data: {ex.Message}");
        }
        return Array.Empty<byte>();
    }

    private bool Pack(byte[] packedData) {
        if (packedData == null || packedData.Length == 0) {
            return false;
        }
        Crc32 crc32 = new();
        crc32.Append(packedData);
        byte[] hash = crc32.GetHashAndReset();
        if ((this.Attribute & PackedAttribute.Compress) == PackedAttribute.Compress) {
            try {
                using MemoryStream resultStream = new();
                using (MemoryStream ms = new(packedData)) {
                    using GZipStream gZipStream = new(resultStream, CompressionMode.Compress);
                    ms.CopyTo(gZipStream);
                }
                packedData = resultStream.ToArray();
            }
            catch (Exception ex) {
                Console.WriteLine($"Failed to compress data: {ex.Message}");
                return false;
            }
        }
        if ((this.Attribute & PackedAttribute.Crypto) == PackedAttribute.Crypto) {
            try {
                using Aes aes = Aes.Create();
                aes.IV = new byte[16] { 120, 83, 106, 50, 111, 116, 51, 73, 46, 46, 46, 46, 46, 46, 46, 46 };
                aes.Key = new byte[32] { 90, 120, 116, 97, 48, 90, 121, 121, 117, 105, 89, 112, 79, 49, 97, 52, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46 };
                var encryptor = aes.CreateEncryptor();
                using MemoryStream msEncrypt = new();
                using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
                csEncrypt.Write(packedData, 0, packedData.Length);
                csEncrypt.FlushFinalBlock();
                packedData = msEncrypt.ToArray();
            }
            catch (Exception ex) {
                Console.WriteLine($"Failed to encrypt data: {ex.Message}");
                return false;
            }
            if (packedData.Length == 0) {
                Console.WriteLine($"Failed to encrypt data: Encrypted data is empty");
                return false;
            }
        }
        this.BinaryVersion = BitConverter.ToUInt32(hash);
        this.PackedData = packedData;
        return true;
    }

    public bool Pack(SysStoryData sysStoryData) {
        if (sysStoryData == null) {
            return false;
        }
        return this.Pack(JsonSerializer.SerializeToUtf8Bytes(sysStoryData, SysDataJsonContext.Default.StoryData));
    }

    public bool Pack(SysSystemData sysSystemData) {
        if (sysSystemData == null) {
            return false;
        }
        return this.Pack(JsonSerializer.SerializeToUtf8Bytes(sysSystemData, SysDataJsonContext.Default.SystemData));
    }
}
