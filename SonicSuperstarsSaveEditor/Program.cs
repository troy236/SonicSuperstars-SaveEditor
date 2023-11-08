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
        Console.WriteLine("Sonic Superstars Save Editor - Version 1.1.1");
        Console.WriteLine("Always backup your save data before modifying it");
        Console.WriteLine();
        try {
            Console.Title = "Sonic Superstars Save Editor 1.1.1";
        }
        catch { }
        string saveFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Sega", "SonicSuperstars", "Steam", "76561198383833971", "SonicSuperstarsStoryData.bin");
        if (!File.Exists(saveFile)) {
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
            saveData = JsonSerializer.Deserialize(File.ReadAllBytes(saveFile), SysDataJsonContext.Default.SaveData);
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
        Console.WriteLine("4. Copy specific slot");
        Console.WriteLine("5. Copy specific slot to different save file");
        Console.WriteLine();
        int choiceInt;
        while (true) {
            Console.Write("Choice: ");
            var choice = Console.ReadLine();
            if (string.IsNullOrEmpty(choice)) return;
            if (!int.TryParse(choice, out choiceInt) || choiceInt <= 0 || choiceInt > 5) {
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
            return;
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
        }
        else if (choiceInt == 4) {
            CopySlot(sysStoryData);
        }
        else if (choiceInt == 5) {
            string saveFileDest = AskForSavePath();
            if (string.IsNullOrEmpty(saveFileDest)) return;
            CopySlot(sysStoryData, saveFileDest);
            return;
        }
        Console.Write("Save changes? Y/N ");
        if (Console.ReadKey().Key != ConsoleKey.Y) {
            Console.WriteLine();
            return;
        }
        Console.WriteLine();
        try {
            Console.WriteLine("Packing save data");
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

    static string AskForSavePath() {
        while (true) {
            Console.WriteLine("Input path to save data. The filename should be either SonicSuperstarsStoryData or OrionStoryData depending on PC or Console");
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

    static void CopySlot(SysStoryData sysStoryData, string saveFile = "") {
        SaveData saveDataDest = null;
        SysStoryData sysStoryDataDest = null;
        if (!string.IsNullOrEmpty(saveFile)) {
            try {
                Console.WriteLine("Reading save data");
                saveDataDest = JsonSerializer.Deserialize(File.ReadAllBytes(saveFile), SysDataJsonContext.Default.SaveData);
                if (saveDataDest == null) {
                    Console.WriteLine("Failed to deserialize save data");
                    return;
                }
                var saveDataBytes = saveDataDest.Unpack();
                if (saveDataBytes == null || saveDataBytes.Length == 0) return;
                sysStoryDataDest = JsonSerializer.Deserialize(saveDataBytes, SysDataJsonContext.Default.StoryData);
                if (sysStoryDataDest == null || sysStoryDataDest.SaveDatas == null || sysStoryDataDest.SaveDatas.Length == 0) {
                    Console.WriteLine("Not a valid StoryData file");
                    return;
                }
                Console.WriteLine("Read save data successfully");
            }
            catch (Exception ex) {
                Console.WriteLine($"Failed to read save data: {ex.Message}");
                return;
            }
        }
        for (int i = 0; i < sysStoryData.SaveDatas.Length; i++) {
            Console.Write($"Slot {i + 1}: ");
            Console.WriteLine($"{(sysStoryData.SaveDatas[i].IsNonSaved ? "No Data" : "Existing Data Found")}");
        }
        int slotIndexFromInt;
        while (true) {
            Console.Write("Enter Slot ID to copy from (1-5): ");
            var slotIndexFrom = Console.ReadLine();
            if (string.IsNullOrEmpty(slotIndexFrom)) return;
            if (!int.TryParse(slotIndexFrom, out slotIndexFromInt) || slotIndexFromInt <= 0 || slotIndexFromInt > 5) {
                Console.WriteLine("Not a valid slot");
                continue;
            }
            if (sysStoryData.SaveDatas[slotIndexFromInt - 1].IsNonSaved) {
                Console.WriteLine("Cannot copy a blank slot");
                continue;
            }
            break;
        }
        if (!string.IsNullOrEmpty(saveFile)) {
            for (int i = 0; i < sysStoryData.SaveDatas.Length; i++) {
                Console.Write($"Slot {i + 1}: ");
                Console.WriteLine($"{(sysStoryData.SaveDatas[i].IsNonSaved ? "No Data" : "Existing Data Found")}");
            }
        }
        int slotIndexToInt;
        while (true) {
            Console.Write("Enter Slot ID to copy to (1-5): ");
            var slotIndexTo = Console.ReadLine();
            if (string.IsNullOrEmpty(slotIndexTo)) return;
            if (!int.TryParse(slotIndexTo, out slotIndexToInt) || slotIndexToInt <= 0 || slotIndexToInt > 5) {
                Console.WriteLine("Not a valid slot");
                continue;
            }
            if (slotIndexFromInt == slotIndexToInt && string.IsNullOrEmpty(saveFile)) {
                Console.WriteLine("Cannot copy to same slot");
                continue;
            }
            break;
        }
        if (string.IsNullOrEmpty(saveFile)) {
            sysStoryData.SaveDatas[slotIndexToInt - 1] = sysStoryData.SaveDatas[slotIndexFromInt - 1];
            return;
        }
        sysStoryDataDest.SaveDatas[slotIndexToInt - 1] = sysStoryData.SaveDatas[slotIndexFromInt - 1];
        Console.Write("Save changes? Y/N ");
        if (Console.ReadKey().Key != ConsoleKey.Y) {
            Console.WriteLine();
            return;
        }
        Console.WriteLine();
        try {
            Console.WriteLine("Packing save data");
            if (!saveDataDest.Pack(sysStoryDataDest)) return;
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
            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(saveDataDest, SysDataJsonContext.Default.SaveData);
            File.WriteAllBytes(saveFile, bytes);
            Console.WriteLine("Done");
        }
        catch (Exception ex) {
            Console.WriteLine($"Failed to save: {ex.Message}");
            return;
        }
    }
}
