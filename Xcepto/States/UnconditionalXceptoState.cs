using System;
using System.Threading.Tasks;

namespace Xcepto.States
{
    public class UnconditionalXceptoState : XceptoState

    {
        public UnconditionalXceptoState(string name) : base(name)
        {
        }

        public override Task<bool> EvaluateConditionsForTransition(IServiceProvider serviceProvider)
            => Task.FromResult(true);

        public override Task OnEnter(IServiceProvider serviceProvider)
        {
            return Task.CompletedTask;
        }
    }
}