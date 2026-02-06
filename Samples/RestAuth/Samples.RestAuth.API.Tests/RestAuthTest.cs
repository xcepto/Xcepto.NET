using System.Security.Cryptography;
using Samples.RestAuth.API.Requests;
using Samples.RestAuth.API.Responses;
using Samples.RestAuth.API.Tests.Scenarios;
using Xcepto;
using Xcepto.Config;
using Xcepto.Rest;

namespace Samples.RestAuth.API.Tests;

[TestFixture]
public class RestAuthTest
{
    [Test]
    public async Task AuthWithGivenToken_Works()
    {
        var tokenComponents = CreateToken();
        Console.WriteLine($"Using TOKENHASH={Convert.ToHexString(tokenComponents.hashed)}");
        Console.WriteLine($"Using TOKEN={tokenComponents.encoded}");
        
        TimeoutConfig timeoutConfig = new TimeoutConfig(
            TimeSpan.FromSeconds(60), 
            TimeSpan.FromSeconds(10)
        );
        var scenario = new MockedTokenScenario(tokenComponents.hashed);
        await XceptoTest.Given(scenario, timeoutConfig, builder =>
        {
            var restAdapter = builder.RegisterAdapter(new XceptoRestAdapter(tokenComponents.encoded));
            
            restAdapter.PostRequest<AuthenticatedTestRequest, AuthenticatedTestResponse>(
                new Uri($"http://localhost:{scenario.ApiPort}/api/authenticated"),
                new AuthenticatedTestRequest(),
                _ => true);
        });
    }

    private (string encoded, byte[] hashed) CreateToken()
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