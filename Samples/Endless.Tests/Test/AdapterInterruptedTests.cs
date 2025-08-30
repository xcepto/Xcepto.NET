using Endless.Tests.Adapters;
using Endless.Tests.Scenarios;
using Xcepto;

namespace Endless.Tests.Test;

[TestFixture(typeof(LongInitializationAdapter))]
[TestFixture(typeof(LongAddServicesAdapter))]
[TestFixture(typeof(LongCleanupAdapter))]
public class AdapterInterruptedTests<T> where T: XceptoAdapter, new()
{
    private XceptoAdapter _adapter;
    [SetUp]
    public void SetUp()
    {
        _adapter = new T();
    }
    
    [Test]
    public void InterruptedShortlyAfterTimeout()
    {
        Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            await XceptoTest.Given(new InstantaneousScenario(), TimeSpan.FromSeconds(5), builder =>
            {
                builder.RegisterAdapter(_adapter);
            });
        });

    }
}