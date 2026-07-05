using Timeout.Tests.Adapters;
using Timeout.Tests.Scenarios;
using Xcepto;
using Xcepto.Adapters;
using Xcepto.Config;
using Xcepto.Exceptions;
using Xcepto.Strategies;
using Xcepto.Strategies.Execution;

namespace Timeout.Tests.Test;

[Parallelizable]
[TestFixtureSource(nameof(AllFixtures))]
public class AdapterInterruptedTests
{
    private readonly Type _adapterType;
    private XceptoTest _xceptoTest;

    public AdapterInterruptedTests(
        Type adapterType,
        BaseExecutionStrategy executionStrategy)
    {
        _adapterType = adapterType;
        _xceptoTest = new XceptoTest(executionStrategy);
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
            yield return new object[] { state, combo[0] };
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
        var timeoutConfig = new TimeoutConfig(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5));
        Assert.ThrowsAsync<TotalTimeoutException>(async () =>
        {
            await _xceptoTest.GivenWithStrategies(new InstantaneousScenario(), timeoutConfig, builder =>
            {
                builder.RegisterAdapter(_adapter);
            });
        });

    }
}