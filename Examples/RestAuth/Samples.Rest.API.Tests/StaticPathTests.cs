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
using Xcepto.Scenarios;

namespace Samples.Rest.API.Tests;

[TestFixture]
public class StaticPathTests: BaseTest
{
    readonly TimeoutConfig _timeoutConfig = new TimeoutConfig(
        TimeSpan.FromSeconds(60), 
        TimeSpan.FromSeconds(10)
    );
    private readonly MockedTokenScenario _scenario = new MockedTokenScenario(CreateToken().hashed);

    [Test]
    public async Task StaticPath_Get_Works()
    {
        await XceptoTest.Given(_scenario, _timeoutConfig, builder =>
        {
            var rest = builder.RestAdapterBuilder()
                .WithBaseUrl(new Uri($"http://localhost:{_scenario.ApiPort}"))
                .WithSerializer(new NewtonsoftSerializer())
                .Build();

            rest.Get("/api/GetPath/validate")
                .AssertSuccess();
        });
    }
    
    [Test]
    public async Task StaticPath_Post_Works()
    {
        await XceptoTest.Given(_scenario, _timeoutConfig, builder =>
        {
            var rest = builder.RestAdapterBuilder()
                .WithBaseUrl(new Uri($"http://localhost:{_scenario.ApiPort}"))
                .WithSerializer(new NewtonsoftSerializer())
                .Build();

            rest.Post("/api/PostPath/validate")
                .AssertSuccess();
        });
    }
    
        [Test]
    public async Task StaticPath_Patch_Works()
    {
        await XceptoTest.Given(_scenario, _timeoutConfig, builder =>
        {
            var rest = builder.RestAdapterBuilder()
                .WithBaseUrl(new Uri($"http://localhost:{_scenario.ApiPort}"))
                .WithSerializer(new NewtonsoftSerializer())
                .Build();

            rest.Patch("/api/PatchPath/validate")
                .AssertSuccess();
        });
    }

    [Test]
    public async Task StaticPath_Put_Works()
    {
        await XceptoTest.Given(_scenario, _timeoutConfig, builder =>
        {
            var rest = builder.RestAdapterBuilder()
                .WithBaseUrl(new Uri($"http://localhost:{_scenario.ApiPort}"))
                .WithSerializer(new NewtonsoftSerializer())
                .Build();

            rest.Put("/api/PutPath/validate")
                .AssertSuccess();
        });
    }

    [Test]
    public async Task StaticPath_Delete_Works()
    {
        await XceptoTest.Given(_scenario, _timeoutConfig, builder =>
        {
            var rest = builder.RestAdapterBuilder()
                .WithBaseUrl(new Uri($"http://localhost:{_scenario.ApiPort}"))
                .WithSerializer(new NewtonsoftSerializer())
                .Build();

            rest.Delete("/api/DeletePath/validate")
                .AssertSuccess();
        });
    }
}