using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace QWiz.Entities.Enum;

[DataContract]
[JsonConverter(typeof(StringEnumConverter))]
public enum Role
{
    General,
    Reviewer
}