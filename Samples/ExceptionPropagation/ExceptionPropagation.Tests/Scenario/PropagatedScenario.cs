using ExceptionPropagation.Tests.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Timeout.Tests.Providers;
using Xcepto;
using Xcepto.Interfaces;

namespace ExceptionPropagation.Tests.Scenario;

public class PropagatedScenario: AsyncScenario
{
    protected override Task<IServiceCollection> Setup() => Task.FromResult<IServiceCollection>(new ServiceCollection()
        .AddSingleton<ILoggingProvider, LoggingProvider>());

    protected override Task Initialize(IServiceProvider serviceProvider)
    {
        var tcs = new TaskCompletionSource();
        PropagateExceptions(tcs.Task);
        tcs.SetException(new PropagatedException());
        return Task.CompletedTask;
    }

}