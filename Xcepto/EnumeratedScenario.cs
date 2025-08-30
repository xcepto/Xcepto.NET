using System;
using System.Collections;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Xcepto
{
    public abstract class EnumeratedScenario
    {
        public abstract IEnumerator Setup(IServiceCollection serviceCollection);
        public abstract Task Initialize(IServiceProvider serviceProvider);
        public abstract Task Cleanup(IServiceProvider serviceProvider);
    }
}
