using Timeout.Tests.Scenarios;
using Timeout.Tests.States;
using Xcepto;
using Xcepto.States;
using Xcepto.Strategies;
using Xcepto.Strategies.Execution;
using Xcepto.Strategies.Isolation;
using Xcepto.Strategies.Scheduling;

namespace Timeout.Tests.Test;
[TestFixtureSource(nameof(AllFixtures))]
public class StateInterruptedTests
{
    private readonly Type _stateType;

    private XceptoState _state;
    private XceptoTest _xceptoTest;

    public StateInterruptedTests(
        Type stateType,
        IExecutionStrategy executionStrategy,
        ISchedulingStrategy schedulingStrategy,
        IIsolationStrategy isolationStrategy)
    {
        _stateType = stateType;
        _xceptoTest = new XceptoTest(executionStrategy, isolationStrategy, schedulingStrategy);
    }

    [SetUp]
    public void SetUp()
    {
        _state = (XceptoState)Activator.CreateInstance(_stateType, "state")!;
    }

    
    public static IEnumerable<object[]> AllFixtures()
    {
        var stateTypes = new[]
        {
            typeof(LongInitializeState),
            typeof(LongOnEnterState),
            typeof(LongEvaluationState)
        };

        foreach (var state in stateTypes)
        foreach (var combo in StrategyCombinations.AllCombinations())
            yield return new object[] { state, combo[0], combo[1], combo[2] };
    }
    
    [Test]
    public void InterruptedShortlyAfterTimeout()
    {
        Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            await _xceptoTest.GivenWithStrategies(new InstantaneousScenario(), TimeSpan.FromSeconds(1), builder =>
            {
                builder.AddStep(_state);
            });
        });

    }
}