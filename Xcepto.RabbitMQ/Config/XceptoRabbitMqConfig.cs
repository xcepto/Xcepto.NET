using System;
using System.Collections.Generic;

namespace Xcepto.RabbitMQ.Config
{
    public class XceptoRabbitMqConfig
    {
        private Dictionary<string, RabbitMqExchange> _exchanges = new Dictionary<string, RabbitMqExchange>();
        internal IEnumerable<RabbitMqExchange> GetExchanges() => _exchanges.Values;
        internal string Username { get; private set; } = "guest";
        internal string Password { get; private set; } = "guest";
        internal int Port { get; private set; } = 5672;
        internal string Hostname { get; private set; } = "localhost";

        public void SetHostName(String hostname)
        {
            Hostname = hostname;
        }
        
        public void SetPort(int port)
        {
            Port = port;
        }
        
        public void SetUsername(String username)
        {
            Username = username;
        }
        
        public void SetPassword(String password)
        {
            Password = password;
        }
        
        public void AddExchange(RabbitMqExchange rabbitMqExchange)
        {
            _exchanges[rabbitMqExchange.Name] = rabbitMqExchange;
        }
    }
}