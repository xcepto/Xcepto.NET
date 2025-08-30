using Timeout.Tests.Scenarios;
using Timeout.Tests.States;
using Xcepto;

namespace Timeout.Tests.Test;
[TestFixture(typeof(LongInitializeState))]
[TestFixture(typeof(LongOnEnterState))]
[TestFixture(typeof(LongEvaluationState))]
public class StateInterruptedTests<T> where T: XceptoState
{
    private XceptoState _state;
    [SetUp]
    public void SetUp()
    {
        _state = (XceptoState)Activator.CreateInstance(typeof(T), "state")!;
    }
    
    [Test]
    public void InterruptedShortlyAfterTimeout()
    {
        Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            await XceptoTest.Given(new InstantaneousScenario(), TimeSpan.FromSeconds(5), builder =>
            {
                builder.AddStep(_state);
            });
        });

    }
}