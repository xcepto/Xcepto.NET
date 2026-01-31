using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.DependencyInjection;
using Samples.SSR.GUI.Tests.Scenarios;
using Testcontainers.PostgreSql;
using Xcepto;
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
        await XceptoTest.Given(scenario, builder =>
        {
            var ssr = builder.RegisterAdapter(new XceptoSSRAdapter());

            var guiBaseUrl = new Uri($"http://localhost:8082/");
            ssr.Get(guiBaseUrl, async response =>
            {
                var content = await response.Content.ReadAsStringAsync();
                return content.Contains("Welcome");
            });
        });
    }
}