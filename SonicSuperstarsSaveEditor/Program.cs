using System.Text.Json;

namespace SonicSuperstarsSaveEditor;

internal class Program {
    static void Main(string[] args) {
        try {
            Start();
        }
        catch (Exception ex) {
            Console.WriteLine("A unexpected error occurred:");
            Console.WriteLine(ex.ToString());
        }
        Console.WriteLine("Press ENTER to exit");
        Console.ReadLine();
    }

    static void Start() {
        Console.WriteLine("Sonic Superstars Save Editor - Version 1.1.0");
        Console.WriteLine("Always backup your save data before modifying it");
        Console.WriteLine();
        try {
            Console.Title = "Sonic Superstars Save Editor 1.1.0";
        }
        catch { }
        string saveFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Sega", "SonicSuperstars", "Steam", "76561198383833971", "SonicSuperstarsStoryData.bin");
        if (!File.Exists(saveFile)) {
            Console.WriteLine("Input path to save data. The filename should be either SonicSuperstarsStoryData or OrionStoryData depending on PC or Console");
            saveFile = AskForSavePath();
            if (string.IsNullOrEmpty(saveFile)) return;
        }
        else {
            Console.WriteLine($"Found Steam save path: {saveFile}");
            Console.Write("Would you like to use this save file? Y/N ");
            if (Console.ReadKey().Key != ConsoleKey.Y) {
                Console.WriteLine();
                saveFile = AskForSavePath();
                if (string.IsNullOrEmpty(saveFile)) return;
            }
            Console.WriteLine();
        }
        SaveData saveData;
        SysStoryData sysStoryData;
        try {
            saveData = JsonSerializer.Deserialize(File.ReadAllText(saveFile), SysDataJsonContext.Default.SaveData);
            if (saveData == null) {
                Console.WriteLine("Failed to deserialize save data");
                return;
            }
            var saveDataBytes = saveData.Unpack();
            if (saveDataBytes == null || saveDataBytes.Length == 0) return;
            sysStoryData = JsonSerializer.Deserialize(saveDataBytes, SysDataJsonContext.Default.StoryData);
            if (sysStoryData == null || sysStoryData.SaveDatas == null || sysStoryData.SaveDatas.Length == 0) {
                Console.WriteLine("Not a valid StoryData file");
                return;
            }
        }
        catch (Exception ex) {
            Console.WriteLine($"Failed to read save data: {ex.Message}");
            return;
        }
        Console.WriteLine("Read save data successfully");
        Console.WriteLine("------------");
        Console.WriteLine("Global data: (Includes data from deleted slots)");
        Console.WriteLine($"Total Medals collected: {sysStoryData.PlayReportData.BattleMedalTotalNum}");
        Console.WriteLine($"Total Medals used: {sysStoryData.PlayReportData.BattleMedalConsumptionNum}");
        Console.WriteLine("------------");
        Console.WriteLine("Select choice:");
        Console.WriteLine("1. Read all slots");
        Console.WriteLine("2. Read specific slot");
        Console.WriteLine("3. Read/Modify specific slot");
        Console.WriteLine();
        int choiceInt;
        while (true) {
            Console.Write("Choice: ");
            var choice = Console.ReadLine();
            if (string.IsNullOrEmpty(choice)) return;
            if (!int.TryParse(choice, out choiceInt) || choiceInt <= 0 || choiceInt > 3) {
                Console.WriteLine("Not a valid choice");
                continue;
            }
            break;
        }
        if (choiceInt == 1) {
            for (int i = 0; i < sysStoryData.SaveDatas.Length; i++) {
                Console.Write($"Slot {i + 1}: ");
                sysStoryData.SaveDatas[i].PrintInfo();
            }
        }
        else if (choiceInt == 2 || choiceInt == 3) {
            int slotIndexInt;
            while (true) {
                Console.Write("Enter Slot ID (1-5): ");
                var slotIndex = Console.ReadLine();
                if (string.IsNullOrEmpty(slotIndex)) return;
                if (!int.TryParse(slotIndex, out slotIndexInt) || slotIndexInt <= 0 || slotIndexInt > 5) {
                    Console.WriteLine("Not a valid slot");
                    continue;
                }
                break;
            }
            Console.Write($"Slot {slotIndexInt}: ");
            sysStoryData.SaveDatas[slotIndexInt - 1].PrintInfo();
            if (choiceInt != 3) return;
            sysStoryData.SaveDatas[slotIndexInt - 1].DoChanges();
            Console.Write("Save changes? Y/N ");
            if (Console.ReadKey().Key != ConsoleKey.Y) {
                Console.WriteLine();
                return;
            }
            Console.WriteLine();
            Console.WriteLine("Packing save data");
            try {
                if (!saveData.Pack(sysStoryData)) return;
            }
            catch (Exception ex) {
                Console.WriteLine($"Failed to pack: {ex.Message}");
                return;
            }
            try {
                Console.WriteLine("Creating backup");
                File.Copy(saveFile, Path.ChangeExtension(saveFile, ".bak"), true);
            }
            catch (Exception ex) {
                Console.WriteLine($"Failed to create backup file: {ex.Message}");
                Console.Write("Continue with overwriting? Y/N ");
                if (Console.ReadKey().Key != ConsoleKey.Y) {
                    Console.WriteLine();
                    return;
                }
                Console.WriteLine();
            }
            try {
                Console.WriteLine("Saving changes");
                byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(saveData, SysDataJsonContext.Default.SaveData);
                File.WriteAllBytes(saveFile, bytes);
                Console.WriteLine("Done");
            }
            catch (Exception ex) {
                Console.WriteLine($"Failed to save: {ex.Message}");
                return;
            }
        }
    }

    static string AskForSavePath() {
        while (true) {
            Console.Write("Enter path (Leave empty to exit): ");
            string saveFile = Console.ReadLine();
            if (string.IsNullOrEmpty(saveFile)) return string.Empty;
            if (string.IsNullOrEmpty(Path.GetExtension(saveFile))) {
                saveFile = Path.ChangeExtension(saveFile, ".bin");
            }
            if (!File.Exists(saveFile)) {
                Console.WriteLine("File does not exist");
                continue;
            }
            return saveFile;
        }
    }
}
