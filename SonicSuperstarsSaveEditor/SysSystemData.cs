using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SonicSuperstarsSaveEditor;

public enum EGraphicQualityType {
    High,
    Low,
    Custom
}

public class SysSystemData : ISysData {
    public float SeVolume { get; set; }
    public float BgmVolume { get; set; }
    public int LanguageNo { get; set; }
    public int UseDisplayIndex { get; set; }
    public Resolution UseResolution { get; set; }
    public Resolution UseWindowSize { get; set; }
    public int FullScreenMode { get; set; }
    public int FrameRate { get; set; }
    public bool EnableVSync { get; set; }
    public GraphicQualityData highQualityData { get; set; }
    public GraphicQualityData lowQualityData { get; set; }
    public GraphicQualityData customQualityData { get; set; }
    [JsonPropertyName("useGraficQualityType")]
    public EGraphicQualityType useGraphicQualityType { get; set; }
    public int[] AnalogSensitivityList { get; set; }
    public bool IsVibrationP1 { get; set; }
    public bool IsVibrationP2 { get; set; }
    public bool IsVibrationP3 { get; set; }
    public bool IsVibrationP4 { get; set; }
    public bool IsUseGamePadConfigCustom { get; set; }
    public bool IsUseMouseKeyboardConfigCustom { get; set; }
    public KeyboardConfig keyboardConfig { get; set; }
    public int uIButtonType { get; set; }
    public int LastSelectSaveSlot { get; set; }
    public int LastSelectCharacter { get; set; }
    public int LastSelectCharacterSkin { get; set; }
    public int MainMenuSkin { get; set; }
    public bool IsCheckQualityType { get; set; }
    public EGraphicQualityType defaultType { get; set; }
}

public class Resolution {
    public int x { get; set; }
    public int y { get; set; }
}

public class GraphicQualityData {
    public bool IsEnableDepthOfField { get; set; }
    public bool IsEnableBloom { get; set; }
    public bool IsEnableShadow { get; set; }
    public bool IsEnableAntiAreas { get; set; }
}

public class KeyboardConfig {
    public List<KeyConfigData> cKeyConfigDataList { get; set; }
    public List<GamePadConfigData> cGamePadConfigDataList { get; set; }
    public List<ConfigData> keyboardDataList { get; set; }
}

public class KeyConfigData {
    public EAction registAction { get; set; }
    public EUseType useType { get; set; }
    public KeyboardMouseData[] keyboardDataList { get; set; }
}

public class KeyboardMouseData {
    public ulong ulongkey { get; set; }
    public ESafeKey safeKey { get; set; }
    public EMouseButton mouse { get; set; }
    public bool isChangeable { get; set; }

    public AzKey Key {
        get {
            return (AzKey)ulongkey;
        }
    }
}

public class GamePadConfigData {
    public EAction registAction { get; set; }
    public EUseType useType { get; set; }
    public GamePadData[] gamePadDataList { get; set; }
}

public class GamePadData {
    public EButton button { get; set; }
    public bool isChangeable { get; set; }
}

public class ConfigData {
    public EButton button { get; set; }
    public EKey key { get; set; }
}
