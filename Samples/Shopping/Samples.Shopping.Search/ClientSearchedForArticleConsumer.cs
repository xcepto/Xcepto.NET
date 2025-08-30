using MassTransit;
using Samples.Shopping.Events.Backend;
using Samples.Shopping.Events.Telemetry;

namespace Samples.Shopping.Search;

public class ClientSearchedForArticleConsumer: IConsumer<ClientSearchedForArticle>
{
    private IBus _bus;

    public ClientSearchedForArticleConsumer(IBus bus)
    {
        _bus = bus;
    }

    public async Task Consume(ConsumeContext<ClientSearchedForArticle> context)
    {
        string articleName = context.Message.Name;
        
        // Search For article

        await _bus.Publish<SearchSearchedForArticle>(new SearchSearchedForArticle()
        {
            ArticleName = articleName,
            Results = ["Christmas tree", "LED lights", "Christmas bulbs"]
        });
    }
}