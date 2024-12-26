using user.common.Secrets;

namespace user.secretsmanager.Service;
public interface ISecretManagerService
{
    Task<PostgresDbSecrets?> GetPostgresDbSecrets();
    Task<RedisSecrets?> GetRedisSecrets();
}
