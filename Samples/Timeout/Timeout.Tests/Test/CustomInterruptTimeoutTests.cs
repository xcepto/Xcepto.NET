using Timeout.Tests.Scenarios;
using Xcepto;
using Xcepto.Strategies;
using Xcepto.Strategies.Execution;
using Xcepto.Strategies.Isolation;
using Xcepto.Strategies.Scheduling;

namespace Timeout.Tests.Test;

[TestFixtureSource(typeof(StrategyCombinations), nameof(StrategyCombinations.AllCombinations))]
public class CustomInterruptTimeoutTests
{
    
    private XceptoTest _xceptoTest;

    public CustomInterruptTimeoutTests(IExecutionStrategy executionStrategy, ISchedulingStrategy schedulingStrategy, IIsolationStrategy isolationStrategy)
    {
        _xceptoTest = new XceptoTest(executionStrategy, isolationStrategy, schedulingStrategy);
    }

    
    [Test]
    public async Task LongRunningOperationDoesntFailWithCustomInterrupt()
    {
        await _xceptoTest.GivenWithStrategies(new LongInitializationScenario(), TimeSpan.FromSeconds(3), _ => { });
    }
}