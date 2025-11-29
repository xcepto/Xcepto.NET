using System;
using System.Collections;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Xcepto
{
    public abstract class EnumeratedScenario
    {
        protected abstract IEnumerator Setup(IServiceCollection serviceCollection);
        protected abstract Task Initialize(IServiceProvider serviceProvider);
        protected abstract Task Cleanup(IServiceProvider serviceProvider);
        
        internal IEnumerator CallSetup(ServiceCollection services)
        {
            yield return Setup(services);
        }
        internal async Task CallInitialize(IServiceProvider serviceProvider) => await Initialize(serviceProvider);

        internal async Task CallCleanup(IServiceProvider serviceProvider) => await Cleanup(serviceProvider);
    }
}
