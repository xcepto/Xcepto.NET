using Timeout.Tests.Scenarios;
using Xcepto;

namespace Timeout.Tests.Test;

[TestFixture(typeof(LongSetupScenario))]
[TestFixture(typeof(LongInitializationScenario))]
[TestFixture(typeof(LongCleanupScenario))]
public class ScenarioInterruptedTests<T> where T: XceptoScenario, new()
{
    private XceptoScenario _scenario;
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