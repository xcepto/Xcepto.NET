using Endless.Tests.Providers;
using Microsoft.Extensions.DependencyInjection;
using Xcepto;
using Xcepto.Interfaces;

namespace Endless.Tests.Scenarios;

public class LongSetupScenario: Scenario
{
    public override async Task<IServiceCollection> Setup()
    {
        await Task.Delay(TimeSpan.FromSeconds(10));
        return new ServiceCollection()
            .AddSingleton<ILoggingProvider, LoggingProvider>();
    }

    public override Task Initialize(IServiceProvider serviceProvider) => Task.CompletedTask;

    public override Task Cleanup(IServiceProvider serviceProvider) => Task.CompletedTask;
}