using System;
using System.Threading.Tasks;

namespace Xcepto
{
    public class UnconditionalXceptoState : XceptoState

    {
        public UnconditionalXceptoState(string name) : base(name)
        {
        }

        public override bool EvaluateConditionsForTransition(IServiceProvider serviceProvider)
        {
            return true;
        }

        public override Task OnEnter(IServiceProvider serviceProvider)
        {
            return Task.CompletedTask;
        }
    }
}