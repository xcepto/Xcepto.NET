using Timeout.Tests.Adapters;
using Timeout.Tests.Scenarios;
using Xcepto;
using Xcepto.Adapters;
using Xcepto.Strategies;
using Xcepto.Strategies.Execution;
using Xcepto.Strategies.Isolation;
using Xcepto.Strategies.Scheduling;

namespace Timeout.Tests.Test;

[Parallelizable]
[TestFixtureSource(nameof(AllFixtures))]
public class AdapterInterruptedTests
{
    private readonly Type _adapterType;
    private XceptoTest _xceptoTest;

    public AdapterInterruptedTests(
        Type adapterType,
        IExecutionStrategy executionStrategy,
        ISchedulingStrategy schedulingStrategy,
        IIsolationStrategy isolationStrategy)
    {
        _adapterType = adapterType;
        _xceptoTest = new XceptoTest(executionStrategy, isolationStrategy, schedulingStrategy);
    }
    
    public static IEnumerable<object[]> AllFixtures()
    {
        var stateTypes = new[]
        {
            typeof(LongInitializationAdapter),
            typeof(LongCleanupAdapter),
        };

        foreach (var state in stateTypes)
        foreach (var combo in StrategyCombinations.AllCombinations())
            yield return new object[] { state, combo[0], combo[1], combo[2] };
    }
    
    private XceptoAdapter _adapter;
    [SetUp]
    public void SetUp()
    {
        _adapter = (XceptoAdapter)Activator.CreateInstance(_adapterType)!;
    }
    
    [Test]
    public void InterruptedShortlyAfterTimeout()
    {
        Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            await _xceptoTest.GivenWithStrategies(new InstantaneousScenario(), TimeSpan.FromSeconds(1), builder =>
            {
                builder.RegisterAdapter(_adapter);
            });
        });

    }
}