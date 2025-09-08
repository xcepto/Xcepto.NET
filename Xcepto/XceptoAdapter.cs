using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Xcepto
{
    public abstract class XceptoAdapter
    {
        public abstract void AssignBuilder(TransitionBuilder builder);
        protected abstract Task Initialize(IServiceProvider serviceProvider);
        protected abstract Task Cleanup(IServiceProvider serviceProvider);
        protected abstract Task AddServices(IServiceCollection serviceCollection);
        internal async Task CallInitialize(IServiceProvider serviceProvider) => await Initialize(serviceProvider);
        internal async Task CallAddServices(IServiceCollection serviceCollection) => await AddServices(serviceCollection);

        public async Task CallCleanup(IServiceProvider serviceProvider) => await Cleanup(serviceProvider);
    }
}