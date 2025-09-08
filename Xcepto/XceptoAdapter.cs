using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Xcepto
{
    public abstract class XceptoAdapter
    {
        protected abstract Task Initialize(IServiceProvider serviceProvider);
        protected abstract Task Cleanup(IServiceProvider serviceProvider);
        protected abstract Task AddServices(IServiceCollection serviceCollection);

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
        internal async Task CallAddServices(IServiceCollection serviceCollection) => await AddServices(serviceCollection);

        public async Task CallCleanup(IServiceProvider serviceProvider) => await Cleanup(serviceProvider);
    }
}