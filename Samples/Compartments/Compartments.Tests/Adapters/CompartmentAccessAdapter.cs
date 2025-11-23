using Compartments.Tests.States;
using Microsoft.Extensions.DependencyInjection;
using Xcepto;
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
}