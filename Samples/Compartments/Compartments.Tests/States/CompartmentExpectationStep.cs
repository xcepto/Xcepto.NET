using Microsoft.Extensions.DependencyInjection;
using Xcepto;
using Xcepto.Data;
using Xcepto.Repositories;
using Xcepto.Util;

namespace Compartments.Tests.States;

public class CompartmentExpectationStep<TService>: XceptoState
where TService: notnull
{
    private readonly string _compartmentIdentifier;
    private readonly Func<TService, bool> _expectation;

    public CompartmentExpectationStep(string name, string compartmentIdentifier,
        Func<TService, bool> expectation) : base(name)
    {
        _expectation = expectation;
        _compartmentIdentifier = compartmentIdentifier;
    }

    public override Task<bool> EvaluateConditionsForTransition(IServiceProvider serviceProvider)
    {
        var service = serviceProvider.GetCompartmentalizedService<TService>(_compartmentIdentifier);
        return Task.FromResult(_expectation(service));
    }

    public override Task OnEnter(IServiceProvider serviceProvider) => Task.CompletedTask;
}