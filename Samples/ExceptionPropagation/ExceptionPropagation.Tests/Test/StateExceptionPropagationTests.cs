using ExceptionPropagation.Tests.Adapters;
using ExceptionPropagation.Tests.Exceptions;
using ExceptionPropagation.Tests.Scenario;
using ExceptionPropagation.Tests.States;
using Xcepto;

namespace ExceptionPropagation.Tests.Test;

[TestFixture]
public class StateExceptionPropagationTests
{
    [Test]
    public void Unpropagated()
    {
        Assert.DoesNotThrowAsync(async () =>
        {
            await XceptoTest.Given(new SimpleSyncScenario(), builder =>
            {
                builder.AddStep(new UnpropagatedState("unpropagated state"));
            });
        });
    }
    
    [Test]
    public void Propagated()
    {
        Assert.ThrowsAsync<PropagatedException>(async () =>
        {
            await XceptoTest.Given(new SimpleSyncScenario(), builder =>
            {
                builder.AddStep(new PropagatedState("propagated state"));
            });
        });
    }
}