using Microsoft.Extensions.DependencyInjection;
using Timeout.Tests.Providers;
using Xcepto;
using Xcepto.Interfaces;
using Xcepto.Scenarios;

namespace Timeout.Tests.Scenarios;

public class LongCleanupScenario: AsyncScenario
{
    protected override Task<IServiceCollection> Setup()
    {
        return Task.FromResult(new ServiceCollection()
            .AddSingleton<ILoggingProvider, LoggingProvider>());
    }
    protected override Task Cleanup(IServiceProvider serviceProvider) => Task.Delay(TimeSpan.FromSeconds(10));
}