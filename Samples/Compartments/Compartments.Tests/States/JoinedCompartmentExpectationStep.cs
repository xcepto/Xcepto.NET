using Microsoft.Extensions.DependencyInjection;
using Xcepto;
using Xcepto.Data;
using Xcepto.Repositories;
using Xcepto.States;

namespace Compartments.Tests.States;

public class JoinedCompartmentExpectationStep: XceptoState
{
    private string _compartment1;
    private string _compartment2;
    private Func<Compartment, Compartment, bool> _expectation;

    public JoinedCompartmentExpectationStep(string name, string compartment1, string compartment2,
        Func<Compartment, Compartment, bool> expectation) : base(name)
    {
        _expectation = expectation;
        _compartment2 = compartment2;
        _compartment1 = compartment1;
    }

    public override Task<bool> EvaluateConditionsForTransition(IServiceProvider serviceProvider)
    {
        var compartmentRepository = serviceProvider.GetRequiredService<CompartmentRepository>();
        var compartment1 = compartmentRepository.GetCompartment(_compartment1);
        var compartment2 = compartmentRepository.GetCompartment(_compartment2);

        return Task.FromResult(_expectation(compartment1, compartment2));
    }

    public override Task OnEnter(IServiceProvider serviceProvider) => Task.CompletedTask;
}