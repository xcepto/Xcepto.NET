using Timeout.Tests.Scenarios;
using Timeout.Tests.States;
using Xcepto;
using Xcepto.Config;
using Xcepto.Exceptions;
using Xcepto.States;
using Xcepto.Strategies;
using Xcepto.Strategies.Execution;

namespace Timeout.Tests.Test;
[Parallelizable]
[TestFixtureSource(nameof(AllFixtures))]
public class StateInterruptedTests
{
    private readonly Type _stateType;

    private XceptoState _state;
    private XceptoTest _xceptoTest;

    public StateInterruptedTests(
        Type stateType,
        BaseExecutionStrategy executionStrategy)
    {
        _stateType = stateType;
        _xceptoTest = new XceptoTest(executionStrategy);
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
            yield return new object[] { state, combo[0] };
    }
    
    [Test]
    public void InterruptedShortlyAfterTimeout()
    {
        Assert.ThrowsAsync<TotalTimeoutException>(async () =>
        {
            await _xceptoTest.GivenWithStrategies(new InstantaneousScenario(), TimeoutConfig.FromSeconds(1), builder =>
            {
                builder.AddStep(_state);
            });
        });

    }
}