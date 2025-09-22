using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Xcepto.States;

internal class ServiceExpectationState<TService>: XceptoState
where TService: class
{
    private Predicate<TService> _predicate;

    public ServiceExpectationState(string name, Predicate<TService> predicate) : base(name)
    {
        _predicate = predicate;
    }

    public override Task<bool> EvaluateConditionsForTransition(IServiceProvider serviceProvider)
    {
        var service = serviceProvider.GetRequiredService<TService>();
        return Task.FromResult(_predicate(service));
    }

    public override Task OnEnter(IServiceProvider serviceProvider) => Task.CompletedTask;
}