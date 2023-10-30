using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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
        Console.WriteLine("Sonic Superstars Save Editor - Version 1.0.0");
        Console.WriteLine("Always backup your save data before modifying it");
        Console.WriteLine();
        try {
            Console.Title = "Sonic Superstars Save Editor 1.0.0";
        }
        catch { }
        string saveFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Sega", "SonicSuperstars", "Steam", "76561198383833971", "SonicSuperstarsStoryData.bin");
        if (!File.Exists(saveFile)) {
            Console.WriteLine("Input path to save data. The filename should be either SonicSuperstarsStoryData or OrionStoryData depending on PC or Console");
            while (true) {
                Console.Write("Enter path (Leave empty to exit): ");
                saveFile = Console.ReadLine();
                if (string.IsNullOrEmpty(saveFile)) return;
                if (!File.Exists(saveFile)) {
                    Console.WriteLine("File does not exist");
                    continue;
                }
                break;
            }
        }
        else {
            Console.WriteLine($"Found Steam save path: {saveFile}");
            Console.Write("Would you like to use this save file? Y/N ");
            if (Console.ReadKey().Key != ConsoleKey.Y) {
                Console.WriteLine();
                while (true) {
                    Console.Write("Enter path (Leave empty to exit): ");
                    saveFile = Console.ReadLine();
                    if (string.IsNullOrEmpty(saveFile)) return;
                    if (!File.Exists(saveFile)) {
                        Console.WriteLine("File does not exist");
                        continue;
                    }
                    break;
                }
            }
            Console.WriteLine();
        }
        var serializeOptions = new JsonSerializerOptions {
            Converters = {
                new ByteArrayConverter()
            }
        };
        SaveData saveData;
        SysStoryData sysStoryData;
        try {
            saveData = JsonSerializer.Deserialize<SaveData>(File.ReadAllText(saveFile), serializeOptions);
            if (saveData == null) {
                Console.WriteLine("Failed to deserialize save data");
                return;
            }
            var saveDataBytes = saveData.Unpack();
            if (saveDataBytes == null) return;
            sysStoryData = JsonSerializer.Deserialize<SysStoryData>(saveDataBytes);
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
            if (!DoChanges(sysStoryData.SaveDatas[slotIndexInt - 1])) {
                return;
            }
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
                if (Console.ReadKey().Key != ConsoleKey.Y) return;
            }
            try {
                Console.WriteLine("Saving changes");
                string content = JsonSerializer.Serialize(saveData, serializeOptions);
                File.WriteAllText(saveFile, content);
                Console.WriteLine("Done");
            }
            catch (Exception ex) {
                Console.WriteLine($"Failed to save: {ex.Message}");
                return;
            }
        }
    }

    static bool DoChanges(SaveDataStory saveDataStory) {
        if (saveDataStory.IsNonSaved) {
            Console.WriteLine("This slot has no data. You can only modify this slot with a prebuilt template");
            return DoTemplate(saveDataStory);
        }
        while (true) {
            Console.WriteLine("You can modify the following data:");
            Console.WriteLine("1. Use prebuilt template (Set Slot data to completed of any Story Mode)");
            Console.WriteLine("2. Medal count");
            Console.WriteLine("3. Fruit count");
            Console.WriteLine("4. Chaos Emerald count");
            Console.WriteLine("5. Unlock all Shop items");
            Console.WriteLine("6. Swap first play state of Story Mode (Toggles opening cutscene)");
            Console.WriteLine("7. Swap first play state of Trip's Story (Toggles opening cutscene)");
            Console.WriteLine();
            int choiceInt;
            while (true) {
                Console.Write("Choice: ");
                var choice = Console.ReadLine();
                if (string.IsNullOrEmpty(choice)) return false;
                if (!int.TryParse(choice, out choiceInt) || choiceInt <= 0 || choiceInt > 7) {
                    Console.WriteLine("Not a valid choice");
                    continue;
                }
                break;
            }
            string consoleText;
            switch (choiceInt) {
                case 1:
                    DoTemplate(saveDataStory);
                    break;
                case 2:
                    Console.Write("Enter Medal count: ");
                    consoleText = Console.ReadLine();
                    if (string.IsNullOrEmpty(consoleText)) return false;
                    if (!int.TryParse(consoleText, out int medalCount) || medalCount < 0) {
                        Console.WriteLine("Not a valid Medal count");
                        break;
                    }
                    Console.WriteLine($"Medal count set to: {medalCount}");
                    saveDataStory.MedalNum = medalCount;
                    break;
                case 3:
                    Console.Write("Enter Fruit count: ");
                    consoleText = Console.ReadLine();
                    if (string.IsNullOrEmpty(consoleText)) return false;
                    if (!int.TryParse(consoleText, out int fruitCount) || fruitCount < 0) {
                        Console.WriteLine("Not a valid Fruit count");
                        break;
                    }
                    Console.WriteLine($"Fruit count set to: {fruitCount}");
                    saveDataStory.FruitNum = fruitCount;
                    break;
                case 4:
                    Console.Write("Enter Chaos Emerald count: ");
                    consoleText = Console.ReadLine();
                    if (string.IsNullOrEmpty(consoleText)) return false;
                    if (!int.TryParse(consoleText, out int emeraldCount) || emeraldCount < 0 || emeraldCount > 7) {
                        Console.WriteLine("Not a valid Chaos Emerald count");
                        break;
                    }
                    Console.WriteLine($"Chaos Emerald count set to: {emeraldCount}");
                    saveDataStory.AddEmeralds(emeraldCount);
                    break;
                case 5:
                    Console.WriteLine("Unlocked all Shop items");
                    saveDataStory.UnlockAllShopItems();
                    break;
                case 6:
                    Console.WriteLine("Toggling Story Mode first play");
                    saveDataStory.IsNormalFirstPlay = !saveDataStory.IsNormalFirstPlay;
                    break;
                case 7:
                    Console.WriteLine("Toggling Trip's Story first play");
                    saveDataStory.IsTripFirstPlay = !saveDataStory.IsTripFirstPlay;
                    break;
                default:
                    break;
            }
            Console.Write("Would you like to modify other data? Y/N ");
            if (Console.ReadKey().Key != ConsoleKey.Y) {
                Console.WriteLine();
                break;
            }
        }
        return true;
    }

    static bool DoTemplate(SaveDataStory saveDataStory) {
        Console.WriteLine("You will be able to specify Chaos Emerald count, Character Act completion and more on any template");
        Console.WriteLine("Templates that complete a later story also complete any prior story");
        Console.WriteLine("1. Story Mode completed");
        Console.WriteLine("2. Trip's Story completed");
        Console.WriteLine("3. Last Story completed");
        int templateInt;
        while (true) {
            Console.Write("Enter Template: ");
            var template = Console.ReadLine();
            if (string.IsNullOrEmpty(template)) return false;
            if (!int.TryParse(template, out templateInt) || templateInt <= 0 || templateInt > 4) {
                Console.WriteLine("Not a valid template choice");
                continue;
            }
            break;
        }
        int chaosEmeraldInt = 0;
        while (true) {
            Console.Write("Enter Chaos Emerald count: ");
            var chaosEmerald = Console.ReadLine();
            if (string.IsNullOrEmpty(chaosEmerald)) break;
            if (!int.TryParse(chaosEmerald, out chaosEmeraldInt) || chaosEmeraldInt < 0 || chaosEmeraldInt > 7) {
                Console.WriteLine("Not a valid Chaos Emerald count");
                continue;
            }
            break;
        }
        while (true) {
            Console.Write("Enter Medal count: ");
            var medalCount = Console.ReadLine();
            if (string.IsNullOrEmpty(medalCount)) break;
            if (!int.TryParse(medalCount, out int medalCountInt) || medalCountInt < 0) {
                Console.WriteLine("Not a valid Medal count");
                continue;
            }
            saveDataStory.MedalNum = medalCountInt;
            break;
        }
        while (true) {
            Console.Write("Enter Fruit count: ");
            var fruitCount = Console.ReadLine();
            if (string.IsNullOrEmpty(fruitCount)) break;
            if (!int.TryParse(fruitCount, out int fruitCountInt) || fruitCountInt < 0) {
                Console.WriteLine("Not a valid Fruit count");
                continue;
            }
            saveDataStory.FruitNum = fruitCountInt;
            break;
        }
        bool characterActsClear = false;
        bool shopItemsUnlocked = false;
        Console.Write("Character Acts completed on Story Mode? Y/N ");
        if (Console.ReadKey().Key == ConsoleKey.Y) characterActsClear = true;
        Console.WriteLine();
        Console.Write("Unlock all shop items? Y/N ");
        if (Console.ReadKey().Key == ConsoleKey.Y) shopItemsUnlocked = true;
        Console.WriteLine();
        saveDataStory.AddTemplate(templateInt, chaosEmeraldInt, characterActsClear);
        if (shopItemsUnlocked) saveDataStory.UnlockAllShopItems();
        Console.WriteLine("Done");
        return true;
    }
}
