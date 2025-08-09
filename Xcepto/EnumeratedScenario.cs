using System.Collections;
using Microsoft.Extensions.DependencyInjection;

namespace Xcepto
{
    public abstract class EnumeratedScenario
    {
        public abstract IEnumerator Setup(IServiceCollection serviceCollection);
    }
}
