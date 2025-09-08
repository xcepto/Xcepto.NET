using ExceptionPropagation.Tests.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Xcepto;

namespace ExceptionPropagation.Tests.Adapters;

public class UnpropagatedAdapter: XceptoAdapter
{
    protected override Task Initialize(IServiceProvider serviceProvider)
    {
        var task = Task.Run(() =>
        {
            throw new PropagatedException();
        });
        return Task.CompletedTask;
    }
}