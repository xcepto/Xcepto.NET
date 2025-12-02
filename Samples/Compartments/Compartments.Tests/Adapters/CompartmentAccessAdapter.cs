using Compartments.Tests.States;
using Compartments.Tests.Test;
using Microsoft.Extensions.DependencyInjection;
using Xcepto;
using Xcepto.Adapters;
using Xcepto.Data;
using Xcepto.Repositories;

namespace Compartments.Tests.Adapters;

public class CompartmentAccessAdapter: XceptoAdapter
{
    protected override Task Initialize(IServiceProvider serviceProvider)
    {
        serviceProvider.GetRequiredService<CompartmentRepository>();
        return Task.CompletedTask;
    }

    public void JoinedCompartmentExpectation(string compartment1, string compartment2, Func<Compartment, Compartment, bool> expectation)
    {
        AddStep(new JoinedCompartmentExpectationStep("Joined compartment expectation state", compartment1, compartment2, expectation));
    }
    
    public void CompartmentExpectation<TService>(string compartment, Func<TService, bool> expectation)
    where TService: notnull
    {
        AddStep(new CompartmentExpectationStep<TService>(
            $"Compartment expectation state of type {typeof(TService)}", compartment, expectation));
    }

    public void CompartmentServiceAction<TService>(string compartmentIdentifier, Action<TService> action)
    where TService: notnull
    {
        AddStep(new CompartmentServiceActionStep<TService>(
            $"Compartment service action of type {typeof(TService).FullName}", 
            compartmentIdentifier, 
            action
            )
        );
    }
}