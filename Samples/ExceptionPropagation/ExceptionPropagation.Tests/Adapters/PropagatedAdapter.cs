using ExceptionPropagation.Tests.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Xcepto;

namespace ExceptionPropagation.Tests.Adapters;

public class PropagatedAdapter: XceptoAdapter
{
    private TransitionBuilder? _builder;

    public override void AssignBuilder(TransitionBuilder builder)
    {
        _builder = builder;
    }

    protected override Task Initialize(IServiceProvider serviceProvider)
    {
        _builder.PropagateExeptions(Task.Run(() =>
        {
            throw new PropagatedException();
        }));
        return Task.CompletedTask;
    }

    protected override Task Cleanup(IServiceProvider serviceProvider) => Task.CompletedTask;
    protected override Task AddServices(IServiceCollection serviceCollection) => Task.CompletedTask;
}