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