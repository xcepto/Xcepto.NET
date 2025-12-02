using Timeout.Tests.Scenarios;
using Timeout.Tests.States;
using Xcepto;
using Xcepto.Scenarios;
using Xcepto.States;
using Xcepto.Strategies;
using Xcepto.Strategies.Execution;
using Xcepto.Strategies.Isolation;
using Xcepto.Strategies.Scheduling;

namespace Timeout.Tests.Test;

[TestFixtureSource(nameof(AllFixtures))]
public class ScenarioInterruptedTests
{
    private readonly Type _scenario;

    private XceptoState _state;

    public ScenarioInterruptedTests(
        Type scenario,
        IExecutionStrategy executionStrategy,
        ISchedulingStrategy schedulingStrategy,
        IIsolationStrategy isolationStrategy)
    {
        _scenario = scenario;
        _xceptoTest = new XceptoTest(executionStrategy, isolationStrategy, schedulingStrategy);
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
            yield return new object[] { state, combo[0], combo[1], combo[2] };
    }
    
    private AsyncScenario _syncScenario;
    private XceptoTest _xceptoTest;

    [SetUp]
    public void SetUp()
    {
        _syncScenario = (AsyncScenario)Activator.CreateInstance(_scenario)!;
    }
    
    [Test]
    public void InterruptedShortlyAfterTimeout()
    {
        Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            await _xceptoTest.GivenWithStrategies(_syncScenario, TimeSpan.FromSeconds(1), _ => { });
        });
    }
}