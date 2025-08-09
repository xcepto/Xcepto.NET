using MassTransit;
using Samples.Shopping.Events.backend;
using Samples.Shopping.Events.telemetry;

namespace Samples.Shopping.Cart;

public class ClientAddedSelectedArticleToCartConsumer: IConsumer<ClientAddedSelectedArticleToCart>
{
    private IBus _bus;

    public ClientAddedSelectedArticleToCartConsumer(IBus bus)
    {
        _bus = bus;
    }

    public async Task Consume(ConsumeContext<ClientAddedSelectedArticleToCart> context)
    {
        await _bus.Publish<CartAddedArticleToCart>(new CartAddedArticleToCart());
    }
}