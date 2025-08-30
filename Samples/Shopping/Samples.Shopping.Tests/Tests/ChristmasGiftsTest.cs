using Samples.Shopping.Api.Contracts.Routes;
using Samples.Shopping.Events.Backend;
using Samples.Shopping.Tests.Config;
using Samples.Shopping.Tests.Scenarios;
using Xcepto;
using Xcepto.RabbitMQ;
using Xcepto.Rest;

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
        var postUrl = new Uri($"http://localhost:8080/{articleRoute.Path}");

        await XceptoTest.Given(new ChristmasGiftsScenario(), builder =>
        {
            var rest = builder.RegisterAdapter(new XceptoRestAdapter());
            var rabbitMq = builder.RegisterAdapter(new XceptoRabbitMqAdapter(config));

            // When
            rest.PostRequest<SearchForArticleRequest, SearchForArticleResponse>(
                postUrl, searchForArticleRequest,  _ => true);
            
            // Then
            rabbitMq.EventCondition<SearchSearchedForArticle>(
                e => e.ArticleName == searchForArticleRequest.ArticleName);
        });
    }
}
