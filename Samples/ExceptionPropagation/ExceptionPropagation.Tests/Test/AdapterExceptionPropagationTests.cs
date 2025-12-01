using ExceptionPropagation.Tests.Adapters;
using ExceptionPropagation.Tests.Exceptions;
using ExceptionPropagation.Tests.Scenario;
using Xcepto;
using Xcepto.Strategies;
using Xcepto.Strategies.Execution;
using Xcepto.Strategies.Isolation;
using Xcepto.Strategies.Scheduling;

namespace ExceptionPropagation.Tests.Test;

[TestFixtureSource(typeof(StrategyCombinations), nameof(StrategyCombinations.AllCombinations))]
public class AdapterExceptionPropagationTests
{
    private XceptoTest _xceptoTest;

    public AdapterExceptionPropagationTests(IExecutionStrategy executionStrategy, ISchedulingStrategy schedulingStrategy, IIsolationStrategy isolationStrategy)
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
                builder.RegisterAdapter(new UnpropagatedAdapter());
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
                builder.RegisterAdapter(new PropagatedAdapter());
            });
        });
    }
}