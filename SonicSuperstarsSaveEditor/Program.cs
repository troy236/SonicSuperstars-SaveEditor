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
        Console.WriteLine("Sonic Superstars Save Editor - Version 1.0.2");
        Console.WriteLine("Always backup your save data before modifying it");
        Console.WriteLine();
        try {
            Console.Title = "Sonic Superstars Save Editor 1.0.2";
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
            if (saveDataBytes == null || saveDataBytes.Length == 0) return;
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
            if (!DoChanges(sysStoryData.SaveDatas[slotIndexInt - 1])) return;
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
            Console.WriteLine("5. Set world map spawn location (Story Mode)");
            Console.WriteLine("6. Set world map spawn location (Trip's Story)");
            Console.WriteLine("7. Unlock all Shop items");
            Console.WriteLine("8. Unlock specific Shop items");
            Console.WriteLine("9. Swap first play state of Story Mode (Toggles opening cutscene)");
            Console.WriteLine("10. Swap first play state of Trip's Story (Toggles opening cutscene)");
            Console.WriteLine();
            int choiceInt;
            while (true) {
                Console.Write("Choice: ");
                var choice = Console.ReadLine();
                if (string.IsNullOrEmpty(choice)) return false;
                if (!int.TryParse(choice, out choiceInt) || choiceInt <= 0 || choiceInt > 8) {
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
                    Console.WriteLine("1-1. Bridge Island Act 1           7-1. Press Factory Act 1");
                    Console.WriteLine("1-2. Bridge Island Act 2           7-2. Press Factory Act 2");
                    Console.WriteLine("2-1. Speed Jungle Act 1            8-1. Golden Capital Act 1");
                    Console.WriteLine("2-2. Speed Jungle Act Sonic        8-2. Golden Capital Act Knuckles");
                    Console.WriteLine("2-3. Speed Jungle Act 2            8-3. Golden Capital Act 2");
                    Console.WriteLine("3-1. Sky Temple Act 1              9-1. Cyber Station Act 1");
                    Console.WriteLine("4-1. Pinball Carnival Act 1        10-1. Frozen Base Act 1");
                    Console.WriteLine("4-2. Pinball Carnival Act 2        10-2. Frozen Base Act Tails");
                    Console.WriteLine("5-1. Lagoon City Act 1             10-3. Frozen Base Act 2");
                    Console.WriteLine("5-2. Lagoon City Act Amy           11-1. Egg Fortress Act 1");
                    Console.WriteLine("5-3. Lagoon City Act 2             11-2. Egg Fortress Act 2");
                    Console.WriteLine("6-1. Sand Sanctuary Act 1");
                    Console.Write("Enter stage: ");
                    consoleText = Console.ReadLine();
                    if (string.IsNullOrEmpty(consoleText)) return false;
                    consoleText = consoleText.Replace('-', '0');
                    if (!int.TryParse(consoleText, out int worldMapStorySpawn) || worldMapStorySpawn < 101 || worldMapStorySpawn > 1102) {
                        Console.WriteLine("Not a valid spawn location");
                        break;
                    }
                    Console.WriteLine("Setting spawn location");
                    saveDataStory.LastWorldMapActPoint = worldMapStorySpawn;
                    break;
                case 6:
                    Console.WriteLine("1-1. Bridge Island Act 1           7-1. Press Factory Act 1");
                    Console.WriteLine("1-2. Bridge Island Act 2           7-2. Press Factory Act 2");
                    Console.WriteLine("2-1. Speed Jungle Act 1            8-1. Golden Capital Act 1");
                    Console.WriteLine("2-2. Speed Jungle Act 2            8-2. Golden Capital Act 2");
                    Console.WriteLine("2-3. Speed Jungle Act 3            8-3. Golden Capital Act 3");
                    Console.WriteLine("3-1. Sky Temple Act 1              9-1. Cyber Station Act 1");
                    Console.WriteLine("4-1. Pinball Carnival Act 1        10-1. Frozen Base Act 1");
                    Console.WriteLine("4-2. Pinball Carnival Act 2        10-2. Frozen Base Act 2");
                    Console.WriteLine("5-1. Lagoon City Act 1             10-3. Frozen Base Act 3");
                    Console.WriteLine("5-2. Lagoon City Act 2             11-1. Egg Fortress Act 1");
                    Console.WriteLine("5-3. Lagoon City Act 3             11-2. Egg Fortress Act 2");
                    Console.WriteLine("6-1. Sand Sanctuary Act 1");
                    Console.Write("Enter stage: ");
                    consoleText = Console.ReadLine();
                    if (string.IsNullOrEmpty(consoleText)) return false;
                    consoleText = consoleText.Replace('-', '0');
                    if (!int.TryParse(consoleText, out int worldMapTripSpawn) || worldMapTripSpawn < 101 || worldMapTripSpawn > 1102) {
                        Console.WriteLine("Not a valid spawn location");
                        break;
                    }
                    Console.WriteLine("Setting spawn location");
                    saveDataStory.LastWorldMapTripActPoint = worldMapTripSpawn;
                    break;
                case 7:
                    Console.WriteLine("Unlocked all Shop items");
                    saveDataStory.UnlockAllShopItems();
                    break;
                case 8:
                    Console.WriteLine("1. Metal Sonic + Blue             11. Black");
                    Console.WriteLine("2. Metal Tails + Yellow           12. Gold");
                    Console.WriteLine("3. Metal Knuckles + Red           13. Rainbow");
                    Console.WriteLine("4. Metal Amy + Pink               14. Star");
                    Console.WriteLine("5. Metal Trip + Orange            15. Heart");
                    Console.WriteLine("6. Metal NiGHTS                   16. Spring");
                    Console.WriteLine("7. Battle Mask                    17. Emergency Vehicle Light");
                    Console.WriteLine("8. Item Box Head                  18. Chimney");
                    Console.WriteLine("9. Polkadot                       19. Angel Ring");
                    Console.WriteLine("10. Camouflage                    20. Flicky");
                    Console.Write("Enter items: (You can separate by comma to enter multiple ex: 1,6,8) ");
                    consoleText = Console.ReadLine();
                    if (string.IsNullOrEmpty(consoleText)) return false;
                    string[] shopItems = consoleText.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    foreach (string shopItem in shopItems) {
                        if (!int.TryParse(shopItem, out int shopItemInt)) {
                            continue;
                        }
                        try {
                            saveDataStory.UnlockShopItem(shopItemInt);
                        }
                        catch { }
                    }
                    Console.WriteLine("Unlocked");
                    break;
                case 9:
                    Console.WriteLine("Toggling Story Mode first play");
                    saveDataStory.IsNormalFirstPlay = !saveDataStory.IsNormalFirstPlay;
                    break;
                case 10:
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
            Console.WriteLine();
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
            if (!int.TryParse(template, out templateInt) || templateInt <= 0 || templateInt > 3) {
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
