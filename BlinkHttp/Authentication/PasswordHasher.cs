﻿using System.Security.Cryptography;

namespace BlinkHttp.Authentication;

/// <summary>
/// Provides methods to hash password and verify it using PBKDF2 algorithm and predefined settings.
/// </summary>
public static class PasswordHasher
{
    private const int SaltSize = 16;        // 128 bits
    private const int HashSize = 32;        // 256 bits
    private const int Iterations = 10000;

    /// <summary>
    /// Hashes given string using PBKDF2 algorithm. 
    /// </summary>
    public static string HashPassword(string password)
    {
        using RandomNumberGenerator rng = RandomNumberGenerator.Create();

        byte[] salt = new byte[SaltSize];
        rng.GetBytes(salt);

        using Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);

        byte[] hash = pbkdf2.GetBytes(HashSize);

        byte[] hashBytes = new byte[SaltSize + HashSize];
        Array.Copy(salt, 0, hashBytes, 0, SaltSize);
        Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

        return Convert.ToBase64String(hashBytes);
    }

    /// <summary>
    /// Verifies given plain password against given hash using PBKDF2 algorithm. Returns true if given password hashed is the same as given stored hash, otherwise returns false.
    /// </summary>
    public static bool VerifyPassword(string password, string storedHash)
    {
        byte[] hashBytes = Convert.FromBase64String(storedHash);
        byte[] salt = new byte[SaltSize];
        Array.Copy(hashBytes, 0, salt, 0, SaltSize);

        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(HashSize);

        for (int i = 0; i < HashSize; i++)
        {
            if (hashBytes[i + SaltSize] != hash[i])
            {
                return false;
            }
        }

        return true;
    }
}
