using System;

namespace Xcepto.RabbitMQ.Config
{
    public class RabbitMqQueue
    {
        internal Func<string, object> Parser { get; }
        internal bool Durable { get; }
        internal Type SchemaType { get; }
        internal string Name { get; }
        internal string RoutingKey { get; }

        internal RabbitMqQueue(string name, string routingKey, Type schemaType, bool durable = false, Func<string, object> parser = null)
        {
            Parser = parser;
            Durable = durable;
            SchemaType = schemaType;
            RoutingKey = routingKey;
            Name = name;
        }
    }
}