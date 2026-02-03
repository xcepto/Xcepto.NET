using Timeout.Tests.Scenarios;
using Timeout.Tests.States;
using Xcepto;
using Xcepto.Config;
using Xcepto.Exceptions;
using Xcepto.Scenarios;
using Xcepto.States;
using Xcepto.Strategies;
using Xcepto.Strategies.Execution;

namespace Timeout.Tests.Test;

[Parallelizable]
[TestFixtureSource(nameof(AllFixtures))]
public class ScenarioInterruptedTests
{
    private readonly Type _scenario;

    private XceptoState _state;

    public ScenarioInterruptedTests(
        Type scenario,
        BaseExecutionStrategy executionStrategy)
    {
        _scenario = scenario;
        _xceptoTest = new XceptoTest(executionStrategy);
    }
    
    public static IEnumerable<object[]> AllFixtures()
    {
        var stateTypes = new[]
        {
            typeof(LongSetupScenario),
            typeof(LongInitializationScenario),
            typeof(LongCleanupScenario),
        };

        foreach (var state in stateTypes)
        foreach (var combo in StrategyCombinations.AllCombinations())
            yield return new object[] { state, combo[0] };
    }
    
    private XceptoScenario _syncScenario;
    private XceptoTest _xceptoTest;

    [SetUp]
    public void SetUp()
    {
        _syncScenario = (XceptoScenario)Activator.CreateInstance(_scenario)!;
    }
    
    [Test]
    public void InterruptedShortlyAfterTimeout()
    {
        var timeoutConfig = new TimeoutConfig(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5));
        Assert.ThrowsAsync<TotalTimeoutException>(async () =>
        {
            await _xceptoTest.GivenWithStrategies(_syncScenario, timeoutConfig, _ => { });
        });
    }
}