using System.Net;
using Samples.SSR.GUI.Requests;
using Samples.SSR.GUI.Tests.Scenarios;
using Xcepto;
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
        
        await XceptoTest.Given(new SsrGuiScenario(), builder =>
        {
            var ssr = builder.RegisterAdapter(new XceptoSSRAdapter());
            
            ssr.PostAssertions(new Uri($"http://localhost:8082/auth/register"), 
                new RegisterRequest(username, password).ToForm(), responseValidator);
            
            ssr.PostAssertions(new Uri($"http://localhost:8082/auth/login"), 
                new LoginRequest(username, password).ToForm(), responseValidator);
            
            ssr.Get(new Uri($"http://localhost:8082/"), async response =>
            {
                var content = await response.Content.ReadAsStringAsync();
                return content.Contains("Welcome") && content.Contains(username);
            });
        });
    }
}