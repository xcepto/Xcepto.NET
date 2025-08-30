using Endless.Tests.Providers;
using Microsoft.Extensions.DependencyInjection;
using Xcepto;
using Xcepto.Interfaces;

namespace Endless.Tests.Scenarios;

public class LongInitializationScenario: Scenario
{
    public override Task<IServiceCollection> Setup()
    {
        return Task.FromResult(new ServiceCollection()
            .AddSingleton<ILoggingProvider, LoggingProvider>());
    }

    public override Task Initialize(IServiceProvider serviceProvider) => Task.Delay(TimeSpan.FromSeconds(10));

    public override Task Cleanup(IServiceProvider serviceProvider) => Task.CompletedTask;
}