using System;
using RabbitMQ.Client;
using Xcepto.RabbitMQ.Config;

namespace Xcepto.RabbitMQ.Utils
{
    public static class RabbitMqExchangeTypeMap
    {
        public static string ToExchangeTypeString(RabbitMqExchangeType exchangeType)
        {
            switch (exchangeType)
            {
                case RabbitMqExchangeType.Direct:
                    return ExchangeType.Direct; 
                case RabbitMqExchangeType.Fanout:
                    return ExchangeType.Fanout;
                default:
                    throw new ArgumentOutOfRangeException(nameof(exchangeType), exchangeType, null);
            }
        }
    }
}