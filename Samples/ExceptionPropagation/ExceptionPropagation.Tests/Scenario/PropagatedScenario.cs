using ExceptionPropagation.Tests.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Timeout.Tests.Providers;
using Xcepto.Interfaces;

namespace ExceptionPropagation.Tests.Scenario;

public class PropagatedScenario: Xcepto.Scenario
{
    public override Task<IServiceCollection> Setup() => Task.FromResult<IServiceCollection>(new ServiceCollection()
        .AddSingleton<ILoggingProvider, LoggingProvider>());

    public override Task Initialize(IServiceProvider serviceProvider)
    {
        Propagate(Task.Run(() =>
        {
            throw new PropagatedException();
        }));
        return Task.CompletedTask;
    }

    public override Task Cleanup(IServiceProvider serviceProvider) => Task.CompletedTask;
}