using Microsoft.Extensions.DependencyInjection;
using Timeout.Tests.Providers;
using Xcepto;
using Xcepto.Interfaces;

namespace Timeout.Tests.Scenarios;

public class LongSetupScenario: XceptoScenario
{
    protected override async Task<IServiceCollection> Setup()
    {
        await Task.Delay(TimeSpan.FromSeconds(10));
        return new ServiceCollection()
            .AddSingleton<ILoggingProvider, LoggingProvider>();
    }

    protected override Task Initialize(IServiceProvider serviceProvider) => Task.CompletedTask;

    protected override Task Cleanup(IServiceProvider serviceProvider) => Task.CompletedTask;
}