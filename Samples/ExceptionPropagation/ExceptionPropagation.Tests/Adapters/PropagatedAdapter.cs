using ExceptionPropagation.Tests.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Xcepto;

namespace ExceptionPropagation.Tests.Adapters;

public class PropagatedAdapter: XceptoAdapter
{
    protected override Task Initialize(IServiceProvider serviceProvider)
    {
        PropagateExceptions(Task.Run(() =>
        {
            throw new PropagatedException();
        }));
        return Task.CompletedTask;
    }

    protected override Task Cleanup(IServiceProvider serviceProvider) => Task.CompletedTask;
    protected override Task AddServices(IServiceCollection serviceCollection) => Task.CompletedTask;
}