using System;
using Xcepto.States;

namespace Xcepto.Adapters;

public class GenericServiceAdapter: XceptoAdapter
{
    public void ServiceAction<TService>(Action<TService> action)
    where TService: class
    {
        AddStep(new ServiceActionState<TService>($"{typeof(TService).Name} action state", action));
    }

    public void ServiceExpectation<TService>(Predicate<TService> predicate)
    where TService: class
    {
        AddStep(new ServiceExpectationState<TService>($"{typeof(TService).Name} expectation state", predicate));
    }
}