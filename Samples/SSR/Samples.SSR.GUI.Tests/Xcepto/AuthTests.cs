using System.Net;
using Samples.SSR.GUI.Requests;
using Samples.SSR.GUI.Tests.Scenarios;
using Xcepto;
using Xcepto.Config;
using Xcepto.SSR;
using Xcepto.SSR.Extensions;

namespace Samples.SSR.GUI.Tests.Xcepto;

[TestFixture]
public class AuthTests
{
    [Test]
    public async Task RegisterAndLogin()
    {
        var username = "test@test.com";
        var password = "Test1234!";
        
        Func<HttpResponseMessage,Task<Action[]>> responseValidator = async response =>
        {
            var content = await response.Content.ReadAsStringAsync();
            return
            [
                () => Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK)),
                () => Assert.That(content, Does.Not.Contain("id=\"errors\""))
            ];
        };

        var scenario = new SsrGuiScenario();
        var timeoutConfig = new TimeoutConfig(
            TimeSpan.FromSeconds(120), 
            TimeSpan.FromSeconds(10) 
        );
        await XceptoTest.Given(scenario, timeoutConfig, builder =>
        {
            var ssr = builder.RegisterAdapter(new XceptoSSRAdapter());
            
            ssr.PostAssertions(new Uri($"http://localhost:{scenario.GuiPort}/auth/register"), 
                new RegisterRequest(username, password).ToForm(), responseValidator);
            
            ssr.PostAssertions(new Uri($"http://localhost:{scenario.GuiPort}/auth/login"), 
                new LoginRequest(username, password).ToForm(), responseValidator);
            
            ssr.Get(new Uri($"http://localhost:{scenario.GuiPort}/"), async response =>
            {
                var content = await response.Content.ReadAsStringAsync();
                return content.Contains("Welcome") && content.Contains(username);
            });
        });
    }
}