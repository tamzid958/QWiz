using Newtonsoft.Json;

namespace QWiz.Helpers.Exception;

public class ExceptionMessage
{
    [JsonProperty("code")] public int Code { set; get; }

    [JsonProperty("message")] public string? Message { set; get; }
}