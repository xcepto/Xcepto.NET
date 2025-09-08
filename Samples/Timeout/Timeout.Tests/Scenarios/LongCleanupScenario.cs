using Microsoft.Extensions.DependencyInjection;
using Timeout.Tests.Providers;
using Xcepto;
using Xcepto.Interfaces;

namespace Timeout.Tests.Scenarios;

public class LongCleanupScenario: XceptoScenario
{
    public override Task<IServiceCollection> Setup()
    {
        return Task.FromResult(new ServiceCollection()
            .AddSingleton<ILoggingProvider, LoggingProvider>());
    }

    public override Task Initialize(IServiceProvider serviceProvider) => Task.CompletedTask;

    public override Task Cleanup(IServiceProvider serviceProvider) => Task.Delay(TimeSpan.FromSeconds(10));
}