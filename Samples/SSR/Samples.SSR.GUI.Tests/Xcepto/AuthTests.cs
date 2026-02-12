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
        
        var scenario = new SsrGuiScenario();
        var timeoutConfig = new TimeoutConfig(
            TimeSpan.FromSeconds(120), 
            TimeSpan.FromSeconds(10) 
        );
        await XceptoTest.Given(scenario, timeoutConfig, builder =>
        {
            var ssr = builder.SsrAdapterBuilder()
                .WithBaseUrl(new Uri($"http://localhost:{scenario.GuiPort}"))
                .Build();

            ssr.Post("/auth/register")
                .WithFormContent(new RegisterRequest(username, password).ToForm())
                .AssertThatResponseStatus(Is.EqualTo(HttpStatusCode.OK))
                .AssertThatResponseContentString(Does.Not.Contain("id=\"errors\""));
            
            ssr.Post("/auth/login")
                .WithFormContent(new LoginRequest(username, password).ToForm())
                .AssertThatResponseStatus(Is.EqualTo(HttpStatusCode.OK))
                .AssertThatResponseContentString(Does.Not.Contain("id=\"errors\""));

            ssr.Get("/")
                .AssertThatResponseContentString(Does.Contain("Welcome"))
                .AssertThatResponseContentString(Does.Contain(username));
        });
    }
}