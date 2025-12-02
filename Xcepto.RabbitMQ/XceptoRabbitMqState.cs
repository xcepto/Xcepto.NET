using System;
using System.Threading.Tasks;
using Xcepto.States;

namespace Xcepto.RabbitMQ
{
    public class XceptoRabbitMqState : XceptoState
    {
        private Predicate<IServiceProvider> _validation;
        public XceptoRabbitMqState(string name, Predicate<IServiceProvider> validation) : base(name)
        {
            this._validation = validation;
        }

        public override Task<bool> EvaluateConditionsForTransition(IServiceProvider serviceProvider)
        {
            bool success = _validation(serviceProvider);
            return Task.FromResult(success);
        }

        public override Task OnEnter(IServiceProvider serviceProvider)
        {
            return Task.CompletedTask;
        }
    }
}