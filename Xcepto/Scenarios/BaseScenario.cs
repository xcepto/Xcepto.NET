using System;
using System.Threading.Tasks;
using Xcepto.Builder;

namespace Xcepto.Scenarios
{
    public abstract class BaseScenario
    {
        private TransitionBuilder? _builder;

        protected void PropagateExceptions(Task task)
        {
            _builder!.PropagateExceptions(task);
        }
        internal void AssignBuilder(TransitionBuilder builder)
        {
            _builder = builder;
        }

        protected abstract Task<IServiceProvider> BaseSetup();

        protected abstract Task BaseInitialize(IServiceProvider serviceProvider);

        protected abstract Task BaseCleanup(IServiceProvider serviceProvider);
        
        
        internal async Task CallInitialize(IServiceProvider serviceProvider) => await BaseInitialize(serviceProvider);
        internal Task<IServiceProvider> CallSetup() => BaseSetup();

        internal async Task CallCleanup(IServiceProvider serviceProvider) => await BaseCleanup(serviceProvider);
    }
}
