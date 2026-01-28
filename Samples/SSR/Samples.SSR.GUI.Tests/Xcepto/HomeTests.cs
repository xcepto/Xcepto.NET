using Samples.SSR.GUI.Tests.Scenarios;
using Xcepto;
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

            var guiBaseUrl = new Uri($"https://localhost:{scenario.RuntimeService.GuiPort}/");
            ssr.GetRequest(guiBaseUrl, async response =>
            {
                var content = await response.ReadAsStringAsync();
                return content.Contains("Welcome");
            });
        });
    }
}