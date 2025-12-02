using Xcepto;
using Xcepto.States;
using Xcepto.Util;

namespace Compartments.Tests.States;

public class CompartmentServiceActionStep<TService>:XceptoState
where TService: notnull
{
    private string _compartmentIdentifier;
    private Action<TService> _action;

    public CompartmentServiceActionStep(string name, string compartmentIdentifier, Action<TService> action) : base(name)
    {
        _action = action;
        _compartmentIdentifier = compartmentIdentifier;
    }

    public override Task<bool> EvaluateConditionsForTransition(IServiceProvider serviceProvider) =>
        Task.FromResult(true);

    public override Task OnEnter(IServiceProvider serviceProvider)
    {
        var compartmentalizedService = serviceProvider.GetCompartmentalizedService<TService>(_compartmentIdentifier);
        _action(compartmentalizedService);
        return Task.CompletedTask;
    }
}