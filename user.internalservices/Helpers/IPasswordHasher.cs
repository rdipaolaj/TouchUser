namespace user.internalservices.Helpers;
public interface IPasswordHasher
{
    (string HashedPassword, string Salt) HashPassword(string password);
    bool VerifyPassword(string plainPassword, string storedHash, string storedSalt);
}
