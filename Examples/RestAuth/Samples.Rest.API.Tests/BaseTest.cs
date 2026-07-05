using System.Security.Cryptography;

namespace Samples.Rest.API.Tests;

public class BaseTest
{
    protected static (string encoded, byte[] hashed) CreateToken()
    {
        byte[] bytes = new byte[32];
        var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(bytes);
        
        var encoded  = Convert.ToBase64String(bytes)
            .Replace("+", "-")
            .Replace("/", "_")
            .TrimEnd('=');
        
        var hashed = SHA256.Create().ComputeHash(bytes);

        return (encoded, hashed);
    }
}