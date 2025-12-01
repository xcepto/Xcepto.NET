using ExceptionPropagation.Tests.Adapters;
using ExceptionPropagation.Tests.Exceptions;
using ExceptionPropagation.Tests.Scenario;
using ExceptionPropagation.Tests.States;
using Xcepto;
using Xcepto.Strategies;
using Xcepto.Strategies.Execution;
using Xcepto.Strategies.Isolation;
using Xcepto.Strategies.Scheduling;

namespace ExceptionPropagation.Tests.Test;

[TestFixtureSource(typeof(StrategyCombinations), nameof(StrategyCombinations.AllCombinations))]
public class StateExceptionPropagationTests
{
    private XceptoTest _xceptoTest;

    public StateExceptionPropagationTests(IExecutionStrategy executionStrategy, ISchedulingStrategy schedulingStrategy, IIsolationStrategy isolationStrategy)
    {
        _xceptoTest = new XceptoTest(executionStrategy, isolationStrategy, schedulingStrategy);
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