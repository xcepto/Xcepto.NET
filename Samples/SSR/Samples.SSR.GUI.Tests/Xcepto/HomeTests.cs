using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.DependencyInjection;
using Samples.SSR.GUI.Tests.Scenarios;
using Testcontainers.PostgreSql;
using Xcepto;
using Xcepto.Config;
using Xcepto.Interfaces;
using Xcepto.SSR;

namespace Samples.SSR.GUI.Tests.Xcepto;

[TestFixture]
public class HomeTests
{
    [Test]
    public async Task GET_Root_Returns_Welcome()
    {
        var scenario = new SsrGuiScenario();
        var timeoutConfig = new TimeoutConfig(
            TimeSpan.FromSeconds(120), 
            TimeSpan.FromSeconds(10) 
        );
        await XceptoTest.Given(scenario, timeoutConfig, builder =>
        {
            var ssr = builder.RegisterAdapter(new XceptoSSRAdapter());

            var guiBaseUrl = new Uri($"http://localhost:{scenario.GuiPort}/");
            ssr.Get(guiBaseUrl, async response =>
            {
                var content = await response.Content.ReadAsStringAsync();
                return content.Contains("Welcome");
            });
        });
    }
}