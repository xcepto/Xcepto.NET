using System.Net.Http.Headers;
using Samples.Rest.API.Requests;
using Samples.Rest.API.Responses;
using Samples.Rest.API.Tests.Scenarios;
using Xcepto;
using Xcepto.Config;
using Xcepto.NewtonsoftJson;
using Xcepto.Rest.Extensions;

namespace Samples.Rest.API.Tests;

[TestFixture]
public class RestAuthTest: BaseTest
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
            var restAdapter = builder.RestAdapterBuilder()
                .WithHttpClient(new HttpClient()
                {
                    DefaultRequestHeaders =
                    {
                        Authorization = new AuthenticationHeaderValue("Bearer", tokenComponents.encoded)
                    }
                })
                .WithBaseUrl(new Uri($"http://localhost:{scenario.ApiPort}"))
                .WithSerializer(new NewtonsoftSerializer())
                .Build();
            
            restAdapter.Post("/api/authenticated")
                .WithCustomName("Post to /api/authenticated")
                .WithRequestBody(() => new AuthenticatedTestRequest())
                .WithResponseType<AuthenticatedTestResponse>()
                .AssertThatResponse(Is.Not.Null);
        });
    }
    
    
    [Test]
    public async Task AuthWithPromisedToken_Works()
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
            var restAdapter = builder.RestAdapterBuilder()
                .WithBaseUrl(new Uri($"http://localhost:{scenario.ApiPort}"))
                .WithSerializer(new NewtonsoftSerializer())
                .Build();
            
            restAdapter.Post("/api/authenticated")
                .WithCustomName("Post to /api/authenticated")
                .WithBearerTokenClient(() => tokenComponents.encoded)
                .WithRequestBody(() => new AuthenticatedTestRequest())
                .WithResponseType<AuthenticatedTestResponse>()
                .AssertThatResponse(Is.Not.Null);
        });
    }
}