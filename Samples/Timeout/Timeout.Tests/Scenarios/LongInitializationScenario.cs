using Microsoft.Extensions.DependencyInjection;
using Timeout.Tests.Providers;
using Xcepto;
using Xcepto.Interfaces;
using Xcepto.Scenarios;

namespace Timeout.Tests.Scenarios;

public class LongInitializationScenario: AsyncScenario
{
    protected override Task<IServiceCollection> Setup()
    {
        return Task.FromResult(new ServiceCollection()
            .AddSingleton<ILoggingProvider, LoggingProvider>());
    }

    protected override Task Initialize(IServiceProvider serviceProvider) => Task.Delay(TimeSpan.FromSeconds(2));
}