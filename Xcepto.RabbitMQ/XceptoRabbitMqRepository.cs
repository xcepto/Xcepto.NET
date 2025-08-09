using System;
using System.Collections.Generic;

namespace Xcepto.RabbitMQ
{
    public class XceptoRabbitMqRepository
    {
        private Dictionary<string, Queue<object>> _messages = new Dictionary<string, Queue<object>>();
        public void EnqueueMessage(Type type, object message)
        {
            var typeFullName = type.FullName;
            if (!_messages.ContainsKey(typeFullName))
            {
                _messages[typeFullName] = new Queue<object>();
            }
            _messages[typeFullName].Enqueue(message);
        }

        public bool TryDequeueMessage(Type type, out object message)
        {
            message = null;
            if (!_messages.ContainsKey(type.FullName))
                return false;
            var queue = _messages[type.FullName];
            if (queue.Count <= 0)
                return false;
            message = queue.Dequeue();
            return true;
        }
    }
}