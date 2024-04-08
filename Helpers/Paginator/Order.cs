using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace QWiz.Helpers.Paginator;

[DataContract]
[JsonConverter(typeof(StringEnumConverter))]
public enum Order
{
    [EnumMember(Value = "asc")] Asc,
    [EnumMember(Value = "desc")] Desc
}