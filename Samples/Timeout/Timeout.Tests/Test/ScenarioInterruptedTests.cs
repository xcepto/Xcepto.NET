using Timeout.Tests.Scenarios;
using Xcepto;

namespace Timeout.Tests.Test;

[TestFixture(typeof(LongSetupSyncScenario))]
[TestFixture(typeof(LongInitializationSyncScenario))]
[TestFixture(typeof(LongCleanupSyncScenario))]
public class ScenarioInterruptedTests<T> where T: SyncScenario, new()
{
    private SyncScenario _syncScenario;
    [SetUp]
    public void SetUp()
    {
        _syncScenario = new T();
    }
    
    [Test]
    public void InterruptedShortlyAfterTimeout()
    {
        Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            await XceptoTest.Given(_syncScenario, TimeSpan.FromSeconds(5), _ => { });
        });
    }
}