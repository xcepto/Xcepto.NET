using ExceptionPropagation.Tests.Exceptions;
using ExceptionPropagation.Tests.Scenario;
using Xcepto;

namespace ExceptionPropagation.Tests.Test;

[TestFixture]
public class ScenarioExceptionPropagationTests
{
    [Test]
    public void Unpropagated()
    {
        Assert.DoesNotThrowAsync(async () =>
        {
            await XceptoTest.Given(new SimpleScenario(), _ => { });
        });
    }
    
    [Test]
    public void Propagated()
    {
        Assert.ThrowsAsync<PropagatedException>(async () =>
        {
            await XceptoTest.Given(new PropagatedScenario(), _ => { });
        });
    }
}