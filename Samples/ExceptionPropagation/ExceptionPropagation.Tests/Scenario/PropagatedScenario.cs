using ExceptionPropagation.Tests.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Timeout.Tests.Providers;
using Xcepto;
using Xcepto.Interfaces;

namespace ExceptionPropagation.Tests.Scenario;

public class PropagatedScenario: XceptoScenario
{
    protected override Task<IServiceCollection> Setup() => Task.FromResult<IServiceCollection>(new ServiceCollection()
        .AddSingleton<ILoggingProvider, LoggingProvider>());

    protected override Task Initialize(IServiceProvider serviceProvider)
    {
        PropagateExceptions(Task.Run(() =>
        {
            throw new PropagatedException();
        }));
        return Task.CompletedTask;
    }

}