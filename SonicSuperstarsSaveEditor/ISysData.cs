using System.Text.Json.Serialization;

namespace SonicSuperstarsSaveEditor;
[JsonDerivedType(typeof(SysStoryData))]
[JsonDerivedType(typeof(SysSystemData))]
internal interface ISysData {
}
