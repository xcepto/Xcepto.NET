using Endless.Tests.Scenarios;
using Xcepto;

namespace Endless.Tests.Test;

[TestFixture(typeof(LongSetupScenario))]
[TestFixture(typeof(LongInitializationScenario))]
[TestFixture(typeof(LongCleanupScenario))]
public class ScenarioInterruptedTests<T> where T: Scenario, new()
{
    private Scenario _scenario;
    [SetUp]
    public void SetUp()
    {
        _scenario = new T();
    }
    
    [Test]
    public void InterruptedShortlyAfterTimeout()
    {
        Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            await XceptoTest.Given(_scenario, TimeSpan.FromSeconds(5), _ => { });
        });
    }
}