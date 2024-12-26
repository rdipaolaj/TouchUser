using System.Text.Json.Serialization;
using user.common.Settings;

namespace user.common.Secrets;
public class RedisSecrets : ISecret
{
    [JsonPropertyName("private-key")]
    public string PrivateKey { get; set; } = string.Empty;
}