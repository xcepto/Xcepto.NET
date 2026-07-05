using NUnit.Framework;
using NUnit.Framework.Constraints;
using Xcepto;
using Xcepto.Builder;
using Xcepto.Config;
using Xcepto.Data;
using Xcepto.NewtonsoftJson;
using Xcepto.Rest.Extensions;
using Xcepto.SSR.Extensions;

namespace Xcepto.Docs.Examples;

[TestFixture]
public class HomepageSnippets
{
    private static readonly TimeoutConfig TIMEOUT = new TimeoutConfig(
        TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(25));

    // Helper used in Promise snippet
    private static string ExtractToken(string page) => page;

    [Test]
    public async Task HeroSnippet_Shipment()
    {
        var scenario = new ShipmentScenario();
        await XceptoTest.Given(scenario, builder =>
        {
            var rest = builder.RestAdapterBuilder()
                .WithBaseUrl(scenario.ApiAddress)
                .WithSerializer(new NewtonsoftSerializer())
                .Build();

            rest.Post("/shipment/accept")
                .WithRequestBody(() => new AcceptShipmentRequest(50))
                .WithResponseType<AcceptShipmentResponse>()
                .AssertThatResponse(r => r.Amount, Is.EqualTo(50));

            // Retried until the assertion passes — no sleep needed
            rest.Get("/inventory/stock")
                .WithResponseType<StockResponse>()
                .AssertThatResponse(r => r.Replenished, Is.True);
        });
    }

    [Test]
    public async Task StateMachine_ConditionsNotTiming()
    {
        var scenario = new ShipmentScenario();
        await XceptoTest.Given(scenario, builder =>
        {
            var rest = builder.RestAdapterBuilder()
                .WithBaseUrl(scenario.ApiAddress)
                .WithSerializer(new NewtonsoftSerializer())
                .Build();

            // POST executes once — assertion failure is immediate
            rest.Post("/shipment/accept")
                .WithRequestBody(() => new AcceptShipmentRequest(50))
                .WithResponseType<AcceptShipmentResponse>()
                .AssertThatResponse(r => r.Amount, Is.EqualTo(50));

            // GET retries until condition is met or timeout expires
            rest.Get("/inventory/stock")
                .WithResponseType<StockResponse>()
                .AssertThatResponse(r => r.Replenished, Is.True);
        });
    }

    [Test]
    public async Task RestAdapter_TypedResponseAndBearerToken()
    {
        var scenario = new ShipmentScenario();
        var username = "user@example.com";
        var password = "password";
        await XceptoTest.Given(scenario, builder =>
        {
            var rest = builder.RestAdapterBuilder()
                .WithBaseUrl(scenario.ApiAddress)
                .WithSerializer(new NewtonsoftSerializer())
                .Build();

            Promise<TokenResponse> token = rest.Post("/auth/login")
                .WithRequestBody(() => new LoginRequest(username, password))
                .WithResponseType<TokenResponse>()
                .AssertThatResponse(r => r.AccessToken, Is.Not.Empty)
                .PromiseResponse();

            rest.Get("/api/profile")
                .WithBearerTokenClient(() => token.Resolve().AccessToken)
                .WithResponseType<ProfileResponse>()
                .AssertThatResponse(r => r.Username, Is.EqualTo(username));
        });
    }

    [Test]
    public async Task SsrAdapter_CookieAwareSession()
    {
        var scenario = new UserScenario();
        var username = "user@example.com";
        var password = "password";
        await XceptoTest.Given(scenario, builder =>
        {
            var ssr = builder.SsrAdapterBuilder()
                .WithBaseUrl(scenario.GuiAddress).Build();

            ssr.Post("/auth/register")
                .WithFormContent(new RegisterRequest(username, password).ToForm())
                .AssertSuccess();

            ssr.Post("/auth/login")
                .WithFormContent(new LoginRequest(username, password).ToForm())
                .AssertSuccess();

            // Session cookie carried automatically — no manual wiring
            ssr.Get("/dashboard")
                .AssertThatResponseContentString(Does.Contain(username));
        });
    }

    [Test]
    public async Task Promise_PassDataBetweenSteps()
    {
        var scenario = new UserScenario();
        await XceptoTest.Given(scenario, builder =>
        {
            var rest = builder.RestAdapterBuilder()
                .WithBaseUrl(scenario.BaseUri)
                .WithSerializer(new NewtonsoftSerializer())
                .Build();

            var ssr = builder.SsrAdapterBuilder()
                .WithBaseUrl(scenario.GuiAddress).Build();

            // A token is embedded in the rendered HTML page
            Promise<string> page = ssr.Post("/token/create")
                .WithFormContent(new TokenCreateRequest("deploy-key").ToForm())
                .AssertSuccess()
                .PromiseResponse();

            // REST call resolves the token lazily at execution time
            rest.Post("/api/env/create")
                .WithBearerTokenClient(() => ExtractToken(page.Resolve()))
                .AssertSuccess();
        });
    }

    [Test]
    public async Task CustomAdapter_DomainDsl()
    {
        var scenario = new ShipmentScenario();
        await XceptoTest.Given(scenario, builder =>
        {
            // In a test — reads as domain behavior
            var orders = new OrderAdapterBuilder(builder).Build();

            orders.Order("order-42")
                .WithAmount(100)
                .ShouldReachStatus(OrderStatus.Fulfilled);
        });
    }
}
