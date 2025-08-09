using Microsoft.Extensions.DependencyInjection;

namespace Xcepto
{
    public abstract class Scenario
    {
        public abstract IServiceCollection Setup();
    }
}
