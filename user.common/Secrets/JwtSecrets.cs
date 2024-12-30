using System.Text.Json.Serialization;
using user.common.Settings;

namespace user.common.Secrets;
public class JwtSecrets : ISecret
{
    [JsonPropertyName("jwt-signing-key")]
    public string JwtSigningKey { get; set; } = string.Empty;
}