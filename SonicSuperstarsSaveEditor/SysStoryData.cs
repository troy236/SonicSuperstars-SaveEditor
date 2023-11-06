using System.Text.Json.Serialization;

namespace SonicSuperstarsSaveEditor;
public class SysStoryData {
    public SaveDataStory[] SaveDatas { get; set; }
    public PlayReportData PlayReportData { get; set; }
    public ulong DLCBuyFlag { get; set; }
    public int mainMenuDLCBackImageType { get; set; }
    public OfflineTimeAttackData OfflineTimeAttackData { get; set; }
}

public enum EquipmentType {
    Root,
    Head,
    Body,
    ArmLeft,
    ArmRight,
    LegLeft,
    LegRight,
    Accessory,
    Color,
    Pattern,
    MetalProto,
    FullSet,
    Max
}

public class PlayReportData {
    public int PractiseStageNum { get; set; }
    public int BattleModeNum { get; set; }
    public int TimeAttackModeNum { get; set; }
    public float StoryModePlayTime { get; set; }
    public float BattleModePlayTime { get; set; }
    public float TimeAttackPlayTime { get; set; }
    public float ShopPlayTime { get; set; }
    [JsonPropertyName("CharaPlayTime")]
    public float[] TripPlayTime { get; set; }
    public int BattleMedalTotalNum { get; set; }
    [JsonPropertyName("BattleMedalCnsumptionlNum")]
    public int BattleMedalConsumptionNum { get; set; }
    public int HideDebugModeOpenNum { get; set; }
    public int HideDebugModeSoundTestUse { get; set; }
    public float HideDebugModeSoundTestTime { get; set; }
}

public class OfflineTimeAttackData {
    public bool IsSended { get; set; }
    public int PlayerType { get; set; }
    public float BestTime { get; set; }
    public string StageName { get; set; }
}

public class BattleRoyalData {
    public int Version { get; set; }
    [JsonPropertyName("AssembleData")]
    public AssembledPartsData[] AssembledData { get; set; }
    public int currentSlot { get; set; }
    public List<BattleBotPart> HeadPartsList { get; set; }
    public List<BattleBotPart> BodyPartsList { get; set; }
    public List<BattleBotPart> ArmsPartsList { get; set; }
    public List<BattleBotPart> LegsPartsList { get; set; }
    public List<BattleBotPart> AccessoriesPartsList { get; set; }
    public List<BattleBotPart> PatternPartsList { get; set; }
    public List<BattleBotPart> ColorPartsList { get; set; }
    public List<BattleBotPart> MetalProtoList { get; set; }
    public List<BattleBotPart> FullSetList { get; set; }
    public float TotalDistance { get; set; }
    public int TotalJumpsCount { get; set; }
    public int TotalItemsAcquired { get; set; }
    public int TotalScore { get; set; }
    public int TotalBattle { get; set; }
    public int TotalWinCount { get; set; }
    public int TotalExp { get; set; }
    public int specialMapLotteryCount { get; set; }
    public int PartsPurchasedNum { get; set; }
    public PartsUnlockFlag[] UnLockFlags { get; set; }
    public List<int> ShopNotificationPartIDList { get; set; }
}

public class AssembledPartsData {
    public List<BattleBotPart> Data { get; set; }
}

public class BattleBotPart {
    public int ID { get; set; }
    public int ColorID { get; set; }
    public int DecalID { get; set; }
    public int EquID { get; set; }
    public EquipmentType eType { get; set; }
}

public class PartsUnlockFlag {
    public bool IsLock { get; set; }
    public bool IsNotification { get; set; }
}

public class StageBestData {
    public int BestScore { get; set; }
    public float BestTime { get; set; }
    public bool BonusEnemyDefeat { get; set; }
}
