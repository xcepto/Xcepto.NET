using Timeout.Tests.Scenarios;
using Xcepto;
using Xcepto.Config;
using Xcepto.Strategies;
using Xcepto.Strategies.Execution;

namespace Timeout.Tests.Test;

[Parallelizable]
[TestFixtureSource(typeof(StrategyCombinations), nameof(StrategyCombinations.AllCombinations))]
public class CustomInterruptTimeoutTests
{
    
    private XceptoTest _xceptoTest;

    public CustomInterruptTimeoutTests(BaseExecutionStrategy executionStrategy)
    {
        _xceptoTest = new XceptoTest(executionStrategy);
    }

    
    [Test]
    public async Task LongRunningOperationDoesntFailWithCustomInterrupt()
    {
        var timeoutConfig = new TimeoutConfig(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(8));
        await _xceptoTest.GivenWithStrategies(new LongInitializationScenario(), timeoutConfig, _ => { });
    }
}