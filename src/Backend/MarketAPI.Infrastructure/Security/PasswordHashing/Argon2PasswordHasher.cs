using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;
using MarketAPI.Domain.Security.PasswordHashing;

namespace MarketAPI.Infrastructure.Security.PasswordHashing;

internal sealed class Argon2PasswordHasher : IPasswordHasher
{
    private const int DegreeOfParallelism = 1;
    private const int Iterations = 2;
    private const int MemorySize = 1024 * 20; 
    private const int SaltSize = 16; 
    private const int HashSize = 32;
    
    public string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);

        var hash = HashPassword(password, salt);

        var combinedBytes = new byte[hash.Length + salt.Length];

        Array.Copy(salt, 0, combinedBytes, 0, SaltSize);  // salt nos bytes 0–15
        Array.Copy(hash, 0, combinedBytes, SaltSize, HashSize);  // hash nos bytes 16–47

        return Convert.ToBase64String(combinedBytes);
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        
        var combinedBytes = Convert.FromBase64String(passwordHash);

        var salt = new byte[SaltSize];
        var hash = new byte[HashSize];

        Array.Copy(combinedBytes, salt, SaltSize);
        Array.Copy(combinedBytes, SaltSize, hash, 0, HashSize);

        var newHash = HashPassword(password, salt);

        return CryptographicOperations.FixedTimeEquals(hash, newHash);
    }
    
    private byte[] HashPassword(string password, byte[] salt)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);

        var hashAlgorithm = new Argon2id(passwordBytes)
        {
            DegreeOfParallelism = DegreeOfParallelism,
            Iterations = Iterations,
            MemorySize = MemorySize,
            Salt = salt
        };

        return hashAlgorithm.GetBytes(HashSize);
    }
}