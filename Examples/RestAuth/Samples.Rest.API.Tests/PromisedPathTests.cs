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
public class PromisedPathTests: BaseTest
{
    readonly TimeoutConfig _timeoutConfig = new TimeoutConfig(
        TimeSpan.FromSeconds(60), 
        TimeSpan.FromSeconds(10)
    );
    private readonly MockedTokenScenario _scenario = new MockedTokenScenario(CreateToken().hashed);

    [Test]
    public async Task PromisedPathFromResponse_Get_Works()
    {
        await XceptoTest.Given(_scenario, _timeoutConfig, builder =>
        {
            var rest = builder.RestAdapterBuilder()
                .WithBaseUrl(new Uri($"http://localhost:{_scenario.ApiPort}"))
                .WithSerializer(new NewtonsoftSerializer())
                .Build();

            var promiseResponse = rest.Get("/api/GetPath")
                .WithResponseType<PathResponse>()
                .AssertSuccess()
                .PromiseResponse();

            rest.Get(() => promiseResponse.Resolve().Path)
                .AssertSuccess();
        });
    }
    
    [Test]
    public async Task PromisedPathFromResponse_Post_Works()
    {
        await XceptoTest.Given(_scenario, _timeoutConfig, builder =>
        {
            var rest = builder.RestAdapterBuilder()
                .WithBaseUrl(new Uri($"http://localhost:{_scenario.ApiPort}"))
                .WithSerializer(new NewtonsoftSerializer())
                .Build();

            var promiseResponse = rest.Get("/api/PostPath")
                .WithResponseType<PathResponse>()
                .AssertSuccess()
                .PromiseResponse();

            rest.Post(() => promiseResponse.Resolve().Path)
                .AssertSuccess();
        });
    }
    
        [Test]
    public async Task PromisedPathFromResponse_Patch_Works()
    {
        await XceptoTest.Given(_scenario, _timeoutConfig, builder =>
        {
            var rest = builder.RestAdapterBuilder()
                .WithBaseUrl(new Uri($"http://localhost:{_scenario.ApiPort}"))
                .WithSerializer(new NewtonsoftSerializer())
                .Build();

            var promiseResponse = rest.Get("/api/PatchPath")
                .WithResponseType<DynamicPathRequest>()
                .AssertSuccess()
                .PromiseResponse();

            rest.Patch(() => promiseResponse.Resolve().Path)
                .AssertSuccess();
        });
    }

    [Test]
    public async Task PromisedPathFromResponse_Put_Works()
    {
        await XceptoTest.Given(_scenario, _timeoutConfig, builder =>
        {
            var rest = builder.RestAdapterBuilder()
                .WithBaseUrl(new Uri($"http://localhost:{_scenario.ApiPort}"))
                .WithSerializer(new NewtonsoftSerializer())
                .Build();

            var promiseResponse = rest.Get("/api/PutPath")
                .WithResponseType<DynamicPathRequest>()
                .AssertSuccess()
                .PromiseResponse();

            rest.Put(() => promiseResponse.Resolve().Path)
                .AssertSuccess();
        });
    }

    [Test]
    public async Task PromisedPathFromResponse_Delete_Works()
    {
        await XceptoTest.Given(_scenario, _timeoutConfig, builder =>
        {
            var rest = builder.RestAdapterBuilder()
                .WithBaseUrl(new Uri($"http://localhost:{_scenario.ApiPort}"))
                .WithSerializer(new NewtonsoftSerializer())
                .Build();

            var promiseResponse = rest.Get("/api/DeletePath")
                .WithResponseType<DynamicPathRequest>()
                .AssertSuccess()
                .PromiseResponse();

            rest.Delete(() => promiseResponse.Resolve().Path)
                .AssertSuccess();
        });
    }
}