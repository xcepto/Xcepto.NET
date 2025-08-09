using System;
using System.Threading.Tasks;

namespace Xcepto
{
    public abstract class XceptoState
    {
        public override string ToString()
        {
            return Name;
        }

        public XceptoState(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public XceptoState NextXceptoState { get; set; }

        public abstract bool EvaluateConditionsForTransition(IServiceProvider serviceProvider);

        public abstract Task OnEnter(IServiceProvider serviceProvider);
    }
}