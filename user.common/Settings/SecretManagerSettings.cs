namespace user.common.Settings;
public class SecretManagerSettings
{
    public bool Local { get; set; }
    public string Region { get; set; } = string.Empty;
    public bool UseLocalstack { get; set; } = default!;
    public string ServiceURL { get; set; } = default!;
    public string AWSAccessKey { get; set; } = default!;
    public string AWSSecretKey { get; set; } = default!;
    public string ArnPostgresSecrets { get; set; } = string.Empty;
    public string ArnRedisSecrets { get; set; } = string.Empty;
}