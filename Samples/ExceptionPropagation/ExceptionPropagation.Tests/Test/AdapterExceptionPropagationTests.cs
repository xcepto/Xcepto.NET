using ExceptionPropagation.Tests.Adapters;
using ExceptionPropagation.Tests.Exceptions;
using ExceptionPropagation.Tests.Scenario;
using Xcepto;
using Xcepto.Strategies;
using Xcepto.Strategies.Execution;

namespace ExceptionPropagation.Tests.Test;

[TestFixtureSource(typeof(StrategyCombinations), nameof(StrategyCombinations.AllCombinations))]
public class AdapterExceptionPropagationTests
{
    private XceptoTest _xceptoTest;

    public AdapterExceptionPropagationTests(IExecutionStrategy executionStrategy)
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