using System.Text.Json.Serialization;

namespace SonicSuperstarsSaveEditor;

[Flags]
public enum EAct {
    None = 0,
    Bridge_Island_Act1 = 1,
    Bridge_Island_Act2 = 2,
    Speed_Jungle_Act1 = 4,
    Speed_Jungle_ActSonic = 8,
    Speed_Jungle_Act2 = 0x10,
    Sky_Temple_Act1 = 0x20,
    Pinball_Carnival_Act1 = 0x40,
    Pinball_Carnival_Act2 = 0x80,
    Lagoon_City_Act1 = 0x100,
    Lagoon_City_ActAmy = 0x200,
    Lagoon_City_Act2 = 0x400,
    Sand_Sanctuary_Act1 = 0x800,
    Press_Factory_Act1 = 0x1000,
    Press_Factory_Act2 = 0x2000,
    Golden_Capital_Act1 = 0x4000,
    Golden_Capital_ActKnuckles = 0x8000,
    Golden_Capital_Act2 = 0x10000,
    Cyber_Station_Act1 = 0x20000,
    Frozen_Base_Act1 = 0x40000,
    Frozen_Base_ActTails = 0x80000,
    Frozen_Base_Act2 = 0x100000,
    Egg_Fortress_Act1 = 0x200000,
    Egg_Fortress_Act2 = 0x400000,
    Last_Story = 0x800000,
    All = 0xFFFFFF
}

public class SaveDataStory {
    public bool IsNonSaved { get; set; }
    public bool IsNormalFirstPlay { get; set; }
    public int ZoneProgress { get; set; }
    public EAct ClearActProgress { get; set; }
    public int LastWorldMapActPoint { get; set; }
    public int[] ClearMiniActList { get; set; }
    public bool IsTripFirstPlay { get; set; }
    public int ZoneTripModeProgress { get; set; }
    public EAct ClearTripActProgress { get; set; }
    public int LastWorldMapTripActPoint { get; set; }
    public int[] ClearTripMiniActList { get; set; }
    public int ChaosEmeraldNum { get; set; }
    public int[] ChaosEmeraldGetZoneNoList { get; set; }
    public int NextMedalSpecialStageNum { get; set; }
    public int NextBonusStageNum { get; set; }
    public int NextWarpGameNum { get; set; }
    [JsonPropertyName("TicketNum")]
    public int FruitNum { get; set; }
    public int MedalNum { get; set; }
    public int MedalCumulativeNumBonusStage { get; set; }
    [JsonPropertyName("BattleRoyaCoin")]
    public int BattleRoyalCoin { get; set; }
    public int ItemBoxDestructionNum { get; set; }
    public int EggCapsuleUnluckyNum { get; set; }
    public int GoalItemNotFindNum { get; set; }
    public bool[] IsBarrier { get; set; }
    public bool IsBarrierTrip { get; set; }
    public bool IsNotificationOpenTripMode { get; set; }
    public bool IsNotificationOpenLastMode { get; set; }
    public List<int> _miniActKeys { get; set; }
    public List<int> _miniActValues { get; set; }
    public List<string> _bestKeys { get; set; }
    public List<StageBestData> _bestValues { get; set; }
    public int ActClearTotalRing { get; set; }
    public int[] EnemyDefeats { get; set; }
    public int EnemyDefeatsTotal { get; set; }
    [JsonPropertyName("BattleRoyaData")]
    public BattleRoyalData BattleRoyalData { get; set; }

    public void PrintInfo() {
        try {
            if (this.IsNonSaved) {
                Console.WriteLine("No Data");
                return;
            }
            Console.WriteLine();
            Console.WriteLine($"Played Story Mode: {!this.IsNormalFirstPlay}");
            Console.WriteLine($"Story Mode Acts Cleared: {this.ClearActProgress}");
            Console.WriteLine($"Trip's Story Unlocked: {this.IsNotificationOpenTripMode}");
            Console.WriteLine($"Played Trip's Story: {!this.IsTripFirstPlay}");
            Console.WriteLine($"Trip's Story Acts Cleared: {this.ClearTripActProgress}");
            if (this.ChaosEmeraldNum == 7 && this.ClearTripActProgress == (EAct)8388607) 
                Console.WriteLine($"Last Story Unlocked: True");
            else Console.WriteLine($"Last Story Unlocked: False");
            Console.WriteLine("------------");
            Console.WriteLine($"Total Rings Collected: {this.ActClearTotalRing}");
            Console.WriteLine($"Total Enemies Defeated: {this.EnemyDefeatsTotal}");
            Console.WriteLine($"Total Item Boxes Collected: {this.ItemBoxDestructionNum}");
            Console.WriteLine($"Current Medal Count: {this.MedalNum}");
            Console.WriteLine($"Current Fruit Count: {this.FruitNum}");
            Console.WriteLine("------------");
            for (int ii = 0; ii < 7; ii++)
                Console.WriteLine($"Emerald {ii + 1} collected from Zone {this.ChaosEmeraldGetZoneNoList[ii]}");
            Console.WriteLine("------------");
            Console.WriteLine("Fruit Acts:");
            Console.WriteLine($"Bridge Island Fruit Act Clears: {this.ClearMiniActList[0] + this.ClearTripMiniActList[0]}");
            Console.WriteLine($"Pinball Carnival Fruit Act Clears: {this.ClearMiniActList[1] + this.ClearTripMiniActList[1]}");
            Console.WriteLine($"Press Factory Fruit Act Clears: {this.ClearMiniActList[2] + this.ClearTripMiniActList[2]}");
            Console.WriteLine("------------");
            Console.WriteLine("Battle Data:");
            Console.WriteLine($"Total Played: {this.BattleRoyalData.TotalBattle}");
            Console.WriteLine($"Total Won: {this.BattleRoyalData.TotalWinCount}");
            Console.WriteLine($"Total Experience: {this.BattleRoyalData.TotalExp}");
            Console.WriteLine($"Total Run Distance: {this.BattleRoyalData.TotalDistance}");
            Console.WriteLine($"Total Jumps: {this.BattleRoyalData.TotalJumpsCount}");
            Console.WriteLine($"Total Item Boxes: {this.BattleRoyalData.TotalItemsAcquired}");
            Console.WriteLine($"Shop Parts Purchased: {this.BattleRoyalData.PartsPurchasedNum}");
            Console.WriteLine("------------");
        }
        catch (Exception ex) {
            Console.WriteLine($"Failed to print information: {ex}");
        }
    }

    public void DoChanges() {
        if (this.IsNonSaved) {
            Console.WriteLine("This slot has no data. You can only modify this slot with a prebuilt template");
            DoTemplate();
            return;
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
                if (string.IsNullOrEmpty(choice)) return;
                if (!int.TryParse(choice, out choiceInt) || choiceInt <= 0 || choiceInt > 10) {
                    Console.WriteLine("Not a valid choice");
                    continue;
                }
                break;
            }
            string consoleText;
            switch (choiceInt) {
                case 1:
                    DoTemplate();
                    break;
                case 2:
                    Console.Write("Enter Medal count: ");
                    consoleText = Console.ReadLine();
                    if (string.IsNullOrEmpty(consoleText)) return;
                    if (!int.TryParse(consoleText, out int medalCount) || medalCount < 0) {
                        Console.WriteLine("Not a valid Medal count");
                        break;
                    }
                    Console.WriteLine($"Medal count set to: {medalCount}");
                    this.MedalNum = medalCount;
                    break;
                case 3:
                    Console.Write("Enter Fruit count: ");
                    consoleText = Console.ReadLine();
                    if (string.IsNullOrEmpty(consoleText)) return;
                    if (!int.TryParse(consoleText, out int fruitCount) || fruitCount < 0) {
                        Console.WriteLine("Not a valid Fruit count");
                        break;
                    }
                    Console.WriteLine($"Fruit count set to: {fruitCount}");
                    this.FruitNum = fruitCount;
                    break;
                case 4:
                    Console.Write("Enter Chaos Emerald count: ");
                    consoleText = Console.ReadLine();
                    if (string.IsNullOrEmpty(consoleText)) return;
                    if (!int.TryParse(consoleText, out int emeraldCount) || emeraldCount < 0 || emeraldCount > 7) {
                        Console.WriteLine("Not a valid Chaos Emerald count");
                        break;
                    }
                    Console.WriteLine($"Chaos Emerald count set to: {emeraldCount}");
                    this.AddEmeralds(emeraldCount);
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
                    if (string.IsNullOrEmpty(consoleText)) return;
                    consoleText = consoleText.Replace('-', '0');
                    if (!int.TryParse(consoleText, out int worldMapStorySpawn) || worldMapStorySpawn < 101 || worldMapStorySpawn > 1102) {
                        Console.WriteLine("Not a valid spawn location");
                        break;
                    }
                    Console.WriteLine("Setting spawn location");
                    this.LastWorldMapActPoint = worldMapStorySpawn;
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
                    if (string.IsNullOrEmpty(consoleText)) return;
                    consoleText = consoleText.Replace('-', '0');
                    if (!int.TryParse(consoleText, out int worldMapTripSpawn) || worldMapTripSpawn < 101 || worldMapTripSpawn > 1102) {
                        Console.WriteLine("Not a valid spawn location");
                        break;
                    }
                    Console.WriteLine("Setting spawn location");
                    this.LastWorldMapTripActPoint = worldMapTripSpawn;
                    break;
                case 7:
                    Console.WriteLine("Unlocked all Shop items");
                    this.UnlockAllShopItems();
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
                    if (string.IsNullOrEmpty(consoleText)) return;
                    string[] shopItems = consoleText.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    foreach (string shopItem in shopItems) {
                        if (!int.TryParse(shopItem, out int shopItemInt)) {
                            continue;
                        }
                        try {
                            this.UnlockShopItem(shopItemInt);
                        }
                        catch { }
                    }
                    Console.WriteLine("Unlocked");
                    break;
                case 9:
                    Console.WriteLine("Toggling Story Mode first play");
                    this.IsNormalFirstPlay = !this.IsNormalFirstPlay;
                    break;
                case 10:
                    Console.WriteLine("Toggling Trip's Story first play");
                    this.IsTripFirstPlay = !this.IsTripFirstPlay;
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
        return;
    }

    public void DoTemplate() {
        Console.WriteLine("You will be able to specify Chaos Emerald count, Character Act completion and more on any template");
        Console.WriteLine("Templates that complete a later story also complete any prior story");
        Console.WriteLine("1. Story Mode completed");
        Console.WriteLine("2. Trip's Story completed");
        Console.WriteLine("3. Last Story completed");
        int templateInt;
        while (true) {
            Console.Write("Enter Template: ");
            var template = Console.ReadLine();
            if (string.IsNullOrEmpty(template)) return;
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
            this.MedalNum = medalCountInt;
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
            this.FruitNum = fruitCountInt;
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
        this.AddTemplate(templateInt, chaosEmeraldInt, characterActsClear);
        if (shopItemsUnlocked) this.UnlockAllShopItems();
        Console.WriteLine("Done");
        return;
    }

    public void UnlockShopItem(int shopItem) {
        this.BattleRoyalData.UnLockFlags[shopItem].IsLock = false;
    }

    public void UnlockAllShopItems() {
        #region Shop Items
        //this.BattleRoyalData.UnLockFlags[0].IsLock = false; //Unknown. Placeholder? Shows as blank name on notification
        this.BattleRoyalData.UnLockFlags[1].IsLock = false; //Sonic Act
        this.BattleRoyalData.UnLockFlags[2].IsLock = false; //Tails Act
        this.BattleRoyalData.UnLockFlags[3].IsLock = false; //Knuckles Act
        this.BattleRoyalData.UnLockFlags[4].IsLock = false; //Amy Act
        this.BattleRoyalData.UnLockFlags[5].IsLock = false; //Metal Trip - Beat Trip's Story
        this.BattleRoyalData.UnLockFlags[6].IsLock = false; //NiGHTS - Get perfect bonus on checkpoint bonus stage
        this.BattleRoyalData.UnLockFlags[7].IsLock = false; //Battle Mask - Play as Trip
        this.BattleRoyalData.UnLockFlags[8].IsLock = false; //Item Box Head - Break 100 item boxes
        this.BattleRoyalData.UnLockFlags[9].IsLock = false; //Polkadot - Unlocked by spending in shop
        this.BattleRoyalData.UnLockFlags[10].IsLock = false; //Camouflage - Battle mode
        this.BattleRoyalData.UnLockFlags[11].IsLock = false; //Black - Buy all 5 Prototypes
        this.BattleRoyalData.UnLockFlags[12].IsLock = false; //Gold - Battle mode Gold rank
        this.BattleRoyalData.UnLockFlags[13].IsLock = false; //Rainbow - Battle mode Legend rank
        this.BattleRoyalData.UnLockFlags[14].IsLock = false; //Star - Battle mode Super rank
        this.BattleRoyalData.UnLockFlags[15].IsLock = false; //Heart - Battle mode Platinum rank
        this.BattleRoyalData.UnLockFlags[16].IsLock = false; //Spring - Buy 30 parts in shop
        this.BattleRoyalData.UnLockFlags[17].IsLock = false; //Emergency Vehicle Light - Beat any Fruit Act
        this.BattleRoyalData.UnLockFlags[18].IsLock = false; //Chimney - Battle mode
        this.BattleRoyalData.UnLockFlags[19].IsLock = false; //Angel Ring - Buy 100 parts in shop
        this.BattleRoyalData.UnLockFlags[20].IsLock = false; //Flicky - Battle mode Bronze Rank
        //this.BattleRoyalData.UnLockFlags[21].IsLock = false; //Mecha Sonic - DLC
        #endregion
    }

    public void AddTemplate(int templateIndex, int emeraldCount, bool characterActsClear) {
        switch (templateIndex) {
            case 1:
                this.AddEmeralds(emeraldCount);
                this.AddCompletedStoryMode(characterActsClear);
                break;
            case 2:
                this.AddEmeralds(emeraldCount);
                this.AddCompletedStoryMode(characterActsClear);
                this.AddCompletedTripStory();
                if (emeraldCount == 7) {
                    this.IsNotificationOpenLastMode = true;
                }
                break;
            case 3:
                this.AddEmeralds(emeraldCount);
                this.AddCompletedStoryMode(characterActsClear);
                this.AddCompletedTripStory();
                this.AddCompletedLastStory(characterActsClear);
                if (emeraldCount == 7) {
                    this.IsNotificationOpenLastMode = true;
                }
                break;
            default:
                break;
        }
    }

    public void AddEmeralds(int emeraldCount) {
        if (emeraldCount < 0 || emeraldCount > 7) {
            emeraldCount = 0;
        }
        Array.Clear(this.ChaosEmeraldGetZoneNoList, 0, this.ChaosEmeraldGetZoneNoList.Length);
        this.ChaosEmeraldNum = emeraldCount;
        for (int i = 0; i < emeraldCount; i++) {
            this.ChaosEmeraldGetZoneNoList[i] = i + 1;
        }
    }

    public void AddCompletedStoryMode(bool characterActsClear) {
        this.IsNotificationOpenTripMode = true;
        this.IsNonSaved = false;
        this.IsNormalFirstPlay = false;
        this.BattleRoyalData.UnLockFlags[4].IsLock = false;
        this.BattleRoyalData.UnLockFlags[7].IsLock = false;
        this.ZoneProgress = 11;
        this.LastWorldMapActPoint = 1102;
        this._miniActKeys.Clear();
        this._miniActValues.Clear();
        this._miniActKeys.Add(4);
        this._miniActKeys.Add(1);
        this._miniActKeys.Add(7);
        this._miniActValues.Add(2);
        this._miniActValues.Add(1);
        this._miniActValues.Add(1);
        if (characterActsClear) {
            this.BattleRoyalData.UnLockFlags[1].IsLock = false;
            this.BattleRoyalData.UnLockFlags[3].IsLock = false;
            this.BattleRoyalData.UnLockFlags[2].IsLock = false;
            this.ClearActProgress = (EAct)8388607;
        }
        else {
            this.ClearActProgress = (EAct)7831543;
        }
    }

    public void AddCompletedTripStory() {
        this.IsTripFirstPlay = false;
        this.BattleRoyalData.UnLockFlags[5].IsLock = false;
        this.ClearTripActProgress = (EAct)8388607;
        this.ZoneTripModeProgress = 11;
        this.LastWorldMapTripActPoint = 1102;
    }

    public void AddCompletedLastStory(bool characterActsClear) {
        this.BattleRoyalData.UnLockFlags[1].IsLock = false; //Sonic Act. Also awarded on completion of Last Story
        if (characterActsClear) {
            this.ClearActProgress = EAct.All;
        }
        else {
            this.ClearActProgress = (EAct)16220151;
        }
    }
}