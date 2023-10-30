using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SonicSuperstarsSaveEditor;
[JsonDerivedType(typeof(SysStoryData))]
[JsonDerivedType(typeof(SysSystemData))]
internal interface ISysData {
}
