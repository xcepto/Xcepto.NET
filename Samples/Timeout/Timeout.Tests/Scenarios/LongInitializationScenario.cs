using Microsoft.Extensions.DependencyInjection;
using Timeout.Tests.Providers;
using Xcepto;
using Xcepto.Interfaces;

namespace Timeout.Tests.Scenarios;

public class LongInitializationScenario: XceptoScenario
{
    protected override Task<IServiceCollection> Setup()
    {
        return Task.FromResult(new ServiceCollection()
            .AddSingleton<ILoggingProvider, LoggingProvider>());
    }

    protected override Task Initialize(IServiceProvider serviceProvider) => Task.Delay(TimeSpan.FromSeconds(10));

    protected override Task Cleanup(IServiceProvider serviceProvider) => Task.CompletedTask;
}