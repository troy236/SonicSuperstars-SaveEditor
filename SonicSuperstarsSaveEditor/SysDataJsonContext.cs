using System.Text.Json.Serialization;

namespace SonicSuperstarsSaveEditor;
[JsonSerializable(typeof(SaveData), TypeInfoPropertyName = "SaveData")]
[JsonSerializable(typeof(SysStoryData), TypeInfoPropertyName = "StoryData")]
[JsonSerializable(typeof(SysSystemData), TypeInfoPropertyName = "SystemData")]
public partial class SysDataJsonContext : JsonSerializerContext {
}
