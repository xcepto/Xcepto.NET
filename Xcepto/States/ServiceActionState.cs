using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Xcepto.States;

internal class ServiceActionState<TService>: XceptoState
where TService: class
{
    private Action<TService> _serviceAction;

    public ServiceActionState(string name, Action<TService> serviceAction) : base(name)
    {
        _serviceAction = serviceAction;
    }

    public override Task<bool> EvaluateConditionsForTransition(IServiceProvider serviceProvider) =>
        Task.FromResult(true);

    public override Task OnEnter(IServiceProvider serviceProvider)
    {
        var service1 = serviceProvider.GetRequiredService<TService>();
        _serviceAction(service1);
        return Task.CompletedTask;
    }
}