using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Xcepto.RabbitMQ.Config
{
    public class RabbitMqExchange
    {
        internal string Name { get; }
        internal RabbitMqExchangeType Type { get; }
        internal IEnumerable<RabbitMqQueue> GetQueues => Queues.Values;
        protected readonly Dictionary<string, RabbitMqQueue> Queues = new ();
        public bool Durable { get; }

        public RabbitMqExchange(string name, RabbitMqExchangeType type, bool durable)
        {
            Durable = durable;
            Name = name;
            Type = type;
        }

        public void BindEvent<TSchema>(string routingKey, bool durable = true, Func<string, TSchema> parser = null)
        {
            if (parser == null)
                parser = str => (TSchema)JsonConvert.DeserializeObject(str, typeof(TSchema));
            var type = typeof(TSchema);
            var typeName = type.FullName;
            Queues[typeName] = new RabbitMqQueue(typeName, routingKey, type, durable, str => parser(str));
        }
    }
}