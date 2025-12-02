using System;
using System.Threading.Tasks;
using Xcepto.Builder;

namespace Xcepto.States
{
    public abstract class XceptoState
    {
        private TransitionBuilder? _builder;

        public override string ToString()
        {
            return Name;
        }

        protected XceptoState(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public XceptoState? NextXceptoState { get; set; }

        public abstract Task<bool> EvaluateConditionsForTransition(IServiceProvider serviceProvider);

        public abstract Task OnEnter(IServiceProvider serviceProvider);
        public virtual Task Initialize(IServiceProvider serviceProvider) => Task.CompletedTask;

        internal void AssignBuilder(TransitionBuilder builder)
        {
            _builder = builder;
        }
        
        protected void PropagateExceptions(Task task)
        {
            _builder!.PropagateExceptions(task);
        }
    }
}