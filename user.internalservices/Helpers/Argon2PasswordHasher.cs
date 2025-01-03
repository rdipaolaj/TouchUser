﻿using Konscious.Security.Cryptography;
using System.Text;

namespace user.internalservices.Helpers;
public class Argon2PasswordHasher : IPasswordHasher
{
    public (string HashedPassword, string Salt) HashPassword(string password)
    {
        var salt = GenerateSalt();
        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            DegreeOfParallelism = 8,
            MemorySize = 8192,
            Iterations = 4
        };

        return (Convert.ToBase64String(argon2.GetBytes(16)), Convert.ToBase64String(salt));
    }

    public bool VerifyPassword(string plainPassword, string storedHash, string storedSalt)
    {
        var saltBytes = Convert.FromBase64String(storedSalt);

        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(plainPassword))
        {
            Salt = saltBytes,
            DegreeOfParallelism = 8,
            MemorySize = 8192,
            Iterations = 4
        };

        var reHashedPassword = Convert.ToBase64String(argon2.GetBytes(16));

        return reHashedPassword.Equals(storedHash);
    }

    private byte[] GenerateSalt()
    {
        var salt = new byte[16];
        using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
        {
            rng.GetBytes(salt);
        }
        return salt;
    }
}