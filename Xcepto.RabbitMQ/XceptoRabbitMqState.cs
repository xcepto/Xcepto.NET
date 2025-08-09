using System;
using System.Threading.Tasks;

namespace Xcepto.RabbitMQ
{
    public class XceptoRabbitMqState : XceptoState
    {
        private Predicate<IServiceProvider> _validation;
        public XceptoRabbitMqState(string name, Predicate<IServiceProvider> validation) : base(name)
        {
            this._validation = validation;
        }

        public override bool EvaluateConditionsForTransition(IServiceProvider serviceProvider)
        {
            bool success = _validation(serviceProvider);
            return success; // || transitionConditions.Evaluate(serviceProvider);
        }

        public override Task OnEnter(IServiceProvider serviceProvider)
        {
            return Task.CompletedTask;
        }
    }
}