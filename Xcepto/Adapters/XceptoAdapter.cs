using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Xcepto
{
    public abstract class XceptoAdapter
    {
        protected virtual Task Initialize(IServiceProvider serviceProvider) => Task.CompletedTask;
        protected virtual Task Cleanup(IServiceProvider serviceProvider) => Task.CompletedTask;

        private TransitionBuilder? _builder;
        internal void AssignBuilder(TransitionBuilder builder)
        {
            _builder = builder;
        }
        protected void PropagateExceptions(Task task)
        {
            _builder!.PropagateExceptions(task);
        }

        protected void AddStep(XceptoState state)
        {
            _builder!.AddStep(state);
        }

        internal async Task CallInitialize(IServiceProvider serviceProvider) => await Initialize(serviceProvider);

        internal async Task CallCleanup(IServiceProvider serviceProvider) => await Cleanup(serviceProvider);
    }
}