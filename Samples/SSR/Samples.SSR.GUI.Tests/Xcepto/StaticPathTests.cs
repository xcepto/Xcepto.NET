using System.Text.RegularExpressions;
using Samples.SSR.GUI.Tests.Scenarios;
using Xcepto;
using Xcepto.Config;
using Xcepto.Internal.Http.Data;
using Xcepto.SSR.Extensions;

namespace Samples.SSR.GUI.Tests.Xcepto;

[TestFixture]
public class StaticPathTests
{
    private static string ParsePath(string html)
    {
        var regex = new Regex(@"Path:\s*([^<]+)");
        var match = regex.Match(html);

        if (!match.Success)
            throw new Exception($"Path not found in response: {html}"); 
        return match.Groups[1].Value;
    }

    readonly SsrGuiScenario _scenario = new();
    readonly TimeoutConfig _timeoutConfig = new(
        TimeSpan.FromSeconds(120), 
        TimeSpan.FromSeconds(10) 
    );
    
    [Test]
    public async Task PromisedPath_GET_works()
    {
        await XceptoTest.Given(_scenario, _timeoutConfig, builder =>
        {
            var ssr = builder.SsrAdapterBuilder()
                .WithBaseUrl(new Uri($"http://localhost:{_scenario.GuiPort}"))
                .Build();

            ssr.Get("/path/validate/get")
                .AssertSuccess();
        });
    }
    
    [Test]
    public async Task PromisedPath_POST_works()
    {
        await XceptoTest.Given(_scenario, _timeoutConfig, builder =>
        {
            var ssr = builder.SsrAdapterBuilder()
                .WithBaseUrl(new Uri($"http://localhost:{_scenario.GuiPort}"))
                .Build();

            ssr.Post("/path/validate/post")
                .AssertSuccess();
        });
    }
    
    [Test]
    public async Task PromisedPath_PATCH_works()
    {
        await XceptoTest.Given(_scenario, _timeoutConfig, builder =>
        {
            var ssr = builder.SsrAdapterBuilder()
                .WithBaseUrl(new Uri($"http://localhost:{_scenario.GuiPort}"))
                .Build();

            ssr.Request("/path/validate/patch", HttpMethodVerb.Patch)
                .AssertSuccess();
        });
    }
    
    [Test]
    public async Task PromisedPath_PUT_works()
    {
        await XceptoTest.Given(_scenario, _timeoutConfig, builder =>
        {
            var ssr = builder.SsrAdapterBuilder()
                .WithBaseUrl(new Uri($"http://localhost:{_scenario.GuiPort}"))
                .Build();

            ssr.Request("/path/validate/put", HttpMethodVerb.Put)
                .AssertSuccess();
        });
    }
    
    [Test]
    public async Task PromisedPath_DELETE_works()
    {
        await XceptoTest.Given(_scenario, _timeoutConfig, builder =>
        {
            var ssr = builder.SsrAdapterBuilder()
                .WithBaseUrl(new Uri($"http://localhost:{_scenario.GuiPort}"))
                .Build();

            ssr.Request("/path/validate/delete", HttpMethodVerb.Delete)
                .AssertSuccess();
        });
    }
}