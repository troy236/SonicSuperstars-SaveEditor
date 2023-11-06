using System.Text.Json.Serialization;
using System.Text.Json;

namespace SonicSuperstarsSaveEditor;
public class ByteArrayConverter : JsonConverter<byte[]> {
    public override byte[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        List<byte> bytes = new();
        reader.Read();
        while (reader.TokenType != JsonTokenType.EndArray) {
            bytes.Add(reader.GetByte());
            reader.Read();
        }
        return bytes.ToArray();
    }

    public override void Write(Utf8JsonWriter writer, byte[] value, JsonSerializerOptions options) {
        writer.WriteStartArray();

        foreach (var val in value) {
            writer.WriteNumberValue(val);
        }

        writer.WriteEndArray();
    }
}
