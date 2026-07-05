using System.Collections.Concurrent;
using Compartments.Tests.Service;
using Xcepto.States;
using Xcepto.Util;

namespace Compartments.Tests.States;

public class CompartmentAsyncExpectationStep: XceptoState
{
    private string _compartment1Identifier;
    private ConcurrentQueue<int> _values = new ConcurrentQueue<int>();
    private int _expectedValue;

    public CompartmentAsyncExpectationStep(string name, string compartment1Identifier, int expectedValue) : base(name)
    {
        _expectedValue = expectedValue;
        _compartment1Identifier = compartment1Identifier;
    }

    public override Task<bool> EvaluateConditionsForTransition(IServiceProvider serviceProvider)
    {
        while (_values.TryDequeue(out int value))
        {
            if (value.Equals(_expectedValue))
                return Task.FromResult(true);
        }
        return Task.FromResult<bool>(false);
    }

    public override Task OnEnter(IServiceProvider serviceProvider) => Task.CompletedTask;

    public override Task Initialize(IServiceProvider serviceProvider)
    {
        var service1 = serviceProvider.GetCompartmentalizedService<Service1>(_compartment1Identifier);
        _values.Enqueue(service1.GetValue());
        return Task.CompletedTask;
    }
}