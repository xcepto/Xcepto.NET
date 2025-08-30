using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Samples.Shopping.Events.Backend;
using Xcepto.RabbitMQ.Config;

namespace Samples.Shopping.Tests.Config;

public class CustomRabbitMqConfig
{
    public static XceptoRabbitMqConfig GetRabbitMqConfig()
    {
        var config = new XceptoRabbitMqConfig();
        config.SetHostName("localhost");
        config.SetPort(5672);
        config.SetUsername("guest");
        config.SetPassword("guest");
        var eventType = typeof(SearchSearchedForArticle);
        var exchange = new RabbitMqExchange($"{eventType.Namespace}:{eventType.Name}", RabbitMqExchangeType.Fanout, true);
        config.AddExchange(exchange);
        exchange.BindEvent<SearchSearchedForArticle>("", durable: true, parser: str =>
        {
            var root = JObject.Parse(str);
            var message = root["message"].ToString();
            var @event = JsonConvert.DeserializeObject<SearchSearchedForArticle>(message);
            if (@event is null)
                throw new ArgumentException("message was not the correct type");
            return @event;
        });
        return config;
    }
}