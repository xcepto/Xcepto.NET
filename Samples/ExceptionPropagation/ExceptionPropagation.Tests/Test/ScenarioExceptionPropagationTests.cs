using ExceptionPropagation.Tests.Exceptions;
using ExceptionPropagation.Tests.Scenario;
using Xcepto;
using Xcepto.Strategies;
using Xcepto.Strategies.Execution;
using Xcepto.Strategies.Isolation;
using Xcepto.Strategies.Scheduling;

namespace ExceptionPropagation.Tests.Test;

[TestFixtureSource(typeof(StrategyCombinations), nameof(StrategyCombinations.AllCombinations))]
public class ScenarioExceptionPropagationTests
{
    private XceptoTest _xceptoTest;

    public ScenarioExceptionPropagationTests(IExecutionStrategy executionStrategy, ISchedulingStrategy schedulingStrategy, IIsolationStrategy isolationStrategy)
    {
        _xceptoTest = new XceptoTest(executionStrategy, isolationStrategy, schedulingStrategy);
    }
    
    [Test]
    public void Unpropagated()
    {
        Assert.DoesNotThrowAsync(async () =>
        {
            await _xceptoTest.GivenWithStrategies(new SimpleSyncScenario(), _ => { });
        });
    }
    
    [Test]
    public void Propagated()
    {
        Assert.ThrowsAsync<PropagatedException>(async () =>
        {
            await _xceptoTest.GivenWithStrategies(new PropagatedScenario(), _ => { });
        });
    }
}