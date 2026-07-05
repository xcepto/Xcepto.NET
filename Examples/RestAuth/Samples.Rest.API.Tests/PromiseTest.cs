using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using Samples.Rest.API.Requests;
using Samples.Rest.API.Responses;
using Samples.Rest.API.Tests.Scenarios;
using Xcepto;
using Xcepto.Config;
using Xcepto.NewtonsoftJson;
using Xcepto.Rest.Extensions;

namespace Samples.Rest.API.Tests;

[TestFixture]
public class PromiseTest: BaseTest
{
    [Test]
    public async Task PromiseBetweenEndpointsWorks()
    {
        TimeoutConfig timeoutConfig = new TimeoutConfig(
            TimeSpan.FromSeconds(60), 
            TimeSpan.FromSeconds(10)
        );
        var scenario = new MockedTokenScenario(CreateToken().hashed);
        await XceptoTest.Given(scenario, timeoutConfig, builder =>
        {
            var rest = builder.RestAdapterBuilder()
                .WithBaseUrl(new Uri($"http://localhost:{scenario.ApiPort}"))
                .WithSerializer(new NewtonsoftSerializer())
                .Build();

            var promiseResponse = rest.Get("/api/A")
                .WithResponseType<AResponse>()
                .AssertSuccess()
                .PromiseResponse();

            rest.Post("/api/B")
                .WithRequestBody(() => new BRequest(promiseResponse.Resolve().Number))
                .AssertSuccess();
        });
    }
}