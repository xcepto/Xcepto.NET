using Timeout.Tests.Scenarios;
using Xcepto;

namespace Timeout.Tests.Test;

[TestFixture]
public class CustomInterruptTimeoutTests
{
    [Test]
    public async Task LongRunningOperationDoesntFailWithCustomInterrupt()
    {
        await XceptoTest.Given(new LongInitializationScenario(), TimeSpan.FromSeconds(20), _ => { });
    }
}