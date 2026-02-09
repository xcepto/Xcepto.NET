using Samples.Shopping.Api.Contracts.Routes;
using Samples.Shopping.Events.Backend;
using Samples.Shopping.Tests.Config;
using Samples.Shopping.Tests.Scenarios;
using Xcepto;
using Xcepto.NewtonsoftJson;
using Xcepto.RabbitMQ;
using Xcepto.Rest;
using Xcepto.Rest.Extensions;

namespace Samples.Shopping.Tests.Tests;

public class ChristmasGiftsTest
{
    [Test]
    public async Task RunTest1()
    {
        var config = CustomRabbitMqConfig.GetRabbitMqConfig();

        var searchForArticleRequest = new SearchForArticleRequest
        {
            ArticleName = "Christmas Tree"
        };

        SearchForArticleRoute articleRoute = new SearchForArticleRoute();

        await XceptoTest.Given(new ChristmasGiftsSyncScenario(), builder =>
        {
            var rest = builder.RestAdapterBuilder()
                .WithSerializer(new NewtonsoftSerializer())
                .Build();
            var rabbitMq = builder.RegisterAdapter(new XceptoRabbitMqAdapter(config));

            // When
            rest.Post($"/{articleRoute.Path}")
                .WithCustomBaseUrl(new Uri("http://localhost:8080"))
                .WithRequestBody(searchForArticleRequest);
            
            // Then
            rabbitMq.EventCondition<SearchSearchedForArticle>(
                e => e.ArticleName == searchForArticleRequest.ArticleName);
        });
    }
}
