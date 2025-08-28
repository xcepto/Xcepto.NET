using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Xcepto.Exceptions;
using Xcepto.Interfaces;
using Xcepto.RabbitMQ.Config;
using Xcepto.RabbitMQ.Utils;

namespace Xcepto.RabbitMQ
{
    public class XceptoRabbitMqAdapter: XceptoAdapter
    {
        private XceptoRabbitMqConfig _config;
        private TransitionBuilder? _builder;

        public XceptoRabbitMqAdapter(XceptoRabbitMqConfig config)
        {
            _config = config;
        }
        
        public override void AssignBuilder(TransitionBuilder builder)
        {
            _builder = builder;
        }

        public void EventCondition<TEvent>(Predicate<TEvent> predicate)
        {
            if (_builder is null)
                throw new AdapterException("Builder was not assigned yet");
            Predicate<IServiceProvider> validation = serviceProvider =>
            {
                var repository = serviceProvider.GetRequiredService<XceptoRabbitMqRepository>();
                var loggingProvider = serviceProvider.GetRequiredService<ILoggingProvider>();
                var type = typeof(TEvent);
                if (!repository.TryDequeueMessage(type, out var message))
                    return false; 
                if (message is TEvent @event)
                {
                    loggingProvider.LogDebug($"received message of type {type.FullName}");
                    return predicate(@event);
                }
                loggingProvider.LogDebug($"received message, bus was not of type {type.FullName}");
                return false;
            };
            
            _builder.AddStep(new XceptoRabbitMqState("EventConditionStep", validation));
        }

        protected override async Task Initialize(IServiceProvider serviceProvider)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _config.Hostname,
                Port = _config.Port,
                UserName = _config.Username,
                Password = _config.Password,
            };
            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();
            
            foreach (var exchange in _config.GetExchanges())
            {
                await channel.ExchangeDeclareAsync(exchange.Name, RabbitMqExchangeTypeMap.ToExchangeTypeString(exchange.Type), 
                    durable: exchange.Durable);
                foreach (var queue in exchange.GetQueues)
                {
                    await channel.QueueDeclareAsync(queue.Name, durable: queue.Durable);
                    await channel.QueueBindAsync(queue.Name, exchange.Name, queue.RoutingKey);

                    var consumer = new AsyncEventingBasicConsumer(channel);
                    consumer.ReceivedAsync += (_, args) =>
                    {
                        var message = Encoding.UTF8.GetString(args.Body.ToArray());
                        var schema = queue.Parser(message);
                        if (!schema.GetType().IsEquivalentTo(queue.SchemaType))
                            throw new ArgumentException($"deserialized object of type {schema.GetType()} was not {queue.SchemaType}");
                        var repository = serviceProvider.GetRequiredService<XceptoRabbitMqRepository>();
                        repository.EnqueueMessage(queue.SchemaType, schema);
                        return Task.CompletedTask;
                    };
                    await channel.BasicConsumeAsync(queue.Name, true, consumer);
                }
            }
        }

        protected override Task Cleanup() => Task.CompletedTask;

        protected override Task AddServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<XceptoRabbitMqRepository>();
            return Task.CompletedTask;
        }
    }
}