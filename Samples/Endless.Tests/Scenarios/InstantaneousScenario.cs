using Endless.Tests.Providers;
using Microsoft.Extensions.DependencyInjection;
using Xcepto;
using Xcepto.Interfaces;

namespace Endless.Tests.Scenarios;

public class InstantaneousScenario: Scenario
{
    public override Task<IServiceCollection> Setup()
    {
        return Task.FromResult(new ServiceCollection()
            .AddSingleton<ILoggingProvider, LoggingProvider>());
    }

    public override Task Initialize(IServiceProvider serviceProvider) => Task.CompletedTask;

    public override Task Cleanup(IServiceProvider serviceProvider) => Task.CompletedTask;
}