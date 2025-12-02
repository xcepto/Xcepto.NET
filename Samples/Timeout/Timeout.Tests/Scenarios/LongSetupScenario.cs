using Microsoft.Extensions.DependencyInjection;
using Timeout.Tests.Providers;
using Xcepto;
using Xcepto.Interfaces;
using Xcepto.Scenarios;

namespace Timeout.Tests.Scenarios;

public class LongSetupScenario: AsyncScenario
{
    protected override async Task<IServiceCollection> Setup()
    {
        await Task.Delay(TimeSpan.FromSeconds(10));
        return new ServiceCollection()
            .AddSingleton<ILoggingProvider, LoggingProvider>();
    }
}