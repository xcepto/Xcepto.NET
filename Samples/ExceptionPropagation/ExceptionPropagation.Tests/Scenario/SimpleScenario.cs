using Microsoft.Extensions.DependencyInjection;
using Timeout.Tests.Providers;
using Xcepto.Interfaces;

namespace ExceptionPropagation.Tests.Scenario;

public class SimpleScenario: Xcepto.Scenario
{
    public override Task<IServiceCollection> Setup() => Task.FromResult<IServiceCollection>(new ServiceCollection()
        .AddSingleton<ILoggingProvider, LoggingProvider>());

    public override Task Initialize(IServiceProvider serviceProvider) => Task.CompletedTask;

    public override Task Cleanup(IServiceProvider serviceProvider) => Task.CompletedTask;
}