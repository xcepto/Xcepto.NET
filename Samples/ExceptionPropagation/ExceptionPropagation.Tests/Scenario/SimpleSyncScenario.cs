using ExceptionPropagation.Tests.Providers;
using Microsoft.Extensions.DependencyInjection;
using Xcepto;
using Xcepto.Interfaces;
using Xcepto.Scenarios;

namespace ExceptionPropagation.Tests.Scenario;

public class SimpleSyncScenario: AsyncScenario
{
    protected override Task<IServiceCollection> Setup() => Task.FromResult<IServiceCollection>(new ServiceCollection()
        .AddSingleton<ILoggingProvider, LoggingProvider>());

}