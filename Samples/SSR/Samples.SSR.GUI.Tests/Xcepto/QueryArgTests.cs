using System.Text.RegularExpressions;
using Samples.SSR.GUI.Tests.Scenarios;
using Xcepto;
using Xcepto.Config;
using Xcepto.Internal.Http.Data;
using Xcepto.SSR.Extensions;

namespace Samples.SSR.GUI.Tests.Xcepto;

[TestFixture]
public class QueryArgTests
{
    readonly SsrGuiScenario _scenario = new();
    readonly TimeoutConfig _timeoutConfig = new(
        TimeSpan.FromSeconds(120), 
        TimeSpan.FromSeconds(10) 
    );
    
    private static string ParseNextPage(string html)
    {
        var regex = new Regex(@"NextPage:\s*([^<]+)");
        var match = regex.Match(html);

        if (!match.Success)
            throw new Exception($"Path not found in response: {html}"); 
        return match.Groups[1].Value;
    }
    
    [Test]
    public async Task StaticQueryArgs()
    {
        await XceptoTest.Given(_scenario, _timeoutConfig, builder =>
        {
            var ssr = builder.SsrAdapterBuilder()
                .WithBaseUrl(new Uri($"http://localhost:{_scenario.GuiPort}"))
                .Build();

            ssr.Get("/query")
                .AddQueryArgument("page", "1")
                .AssertSuccess()
                .AssertThatResponseContentString(Does.Contain("Page1"))
                .AssertThatResponseContentString(Does.Not.Contain("Page2"));
        });
    }
    
    [Test]
    public async Task DynamicQueryArgs()
    {
        await XceptoTest.Given(_scenario, _timeoutConfig, builder =>
        {
            var ssr = builder.SsrAdapterBuilder()
                .WithBaseUrl(new Uri($"http://localhost:{_scenario.GuiPort}"))
                .Build();

            var promiseResponse = ssr.Get("/query")
                .AddQueryArgument("page", "1")
                .AssertSuccess()
                .AssertThatResponseContentString(Does.Contain("Page1"))
                .AssertThatResponseContentString(Does.Not.Contain("Page2"))
                .PromiseResponse();

            ssr.Get("/query")
                .AddQueryArgument(() => new("page", ParseNextPage(promiseResponse.Resolve())))
                .AssertSuccess()
                .AssertThatResponseContentString(Does.Not.Contain("Page1"))
                .AssertThatResponseContentString(Does.Contain("Page2"));
        });
    }
}