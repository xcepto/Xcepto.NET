using Microsoft.Extensions.DependencyInjection;
using Timeout.Tests.Providers;
using Xcepto;
using Xcepto.Interfaces;
using Xcepto.Scenarios;

namespace Timeout.Tests.Scenarios;

public class InstantaneousScenario: AsyncScenario
{
    protected override Task<IServiceCollection> Setup()
    {
        return Task.FromResult(new ServiceCollection()
            .AddSingleton<ILoggingProvider, LoggingProvider>());
    }
}