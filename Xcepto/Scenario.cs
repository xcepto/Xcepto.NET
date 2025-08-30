using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Xcepto
{
    public abstract class Scenario
    {
        public abstract Task<IServiceCollection> Setup();

        public abstract Task Initialize(IServiceProvider serviceProvider);

        public abstract Task Cleanup(IServiceProvider serviceProvider);
    }
}
