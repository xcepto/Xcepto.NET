using ExceptionPropagation.Tests.Adapters;
using ExceptionPropagation.Tests.Exceptions;
using ExceptionPropagation.Tests.Scenario;
using ExceptionPropagation.Tests.States;
using Xcepto;
using Xcepto.Strategies;
using Xcepto.Strategies.Execution;

namespace ExceptionPropagation.Tests.Test;

[TestFixtureSource(typeof(StrategyCombinations), nameof(StrategyCombinations.AllCombinations))]
public class StateExceptionPropagationTests
{
    private XceptoTest _xceptoTest;

    public StateExceptionPropagationTests(BaseExecutionStrategy executionStrategy)
    {
        _xceptoTest = new XceptoTest(executionStrategy);
    }

    [Test]
    public void Unpropagated()
    {
        Assert.DoesNotThrowAsync(async () =>
        {
            await _xceptoTest.GivenWithStrategies(new SimpleSyncScenario(), builder =>
            {
                builder.AddStep(new UnpropagatedState("unpropagated state"));
            });
        });
    }
    
    [Test]
    public void Propagated()
    {
        Assert.ThrowsAsync<PropagatedException>(async () =>
        {
            await _xceptoTest.GivenWithStrategies(new SimpleSyncScenario(), builder =>
            {
                builder.AddStep(new PropagatedState("propagated state"));
            });
        });
    }
}