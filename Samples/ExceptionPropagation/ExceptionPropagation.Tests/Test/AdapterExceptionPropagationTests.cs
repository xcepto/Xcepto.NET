using ExceptionPropagation.Tests.Adapters;
using ExceptionPropagation.Tests.Exceptions;
using ExceptionPropagation.Tests.Scenario;
using Xcepto;

namespace ExceptionPropagation.Tests.Test;

[TestFixture]
public class AdapterExceptionPropagationTests
{
    [Test]
    public void Unpropagated()
    {
        Assert.DoesNotThrowAsync(async () =>
        {
            await XceptoTest.Given(new SimpleScenario(), builder =>
            {
                builder.RegisterAdapter(new UnpropagatedAdapter());
            });
        });
    }
    
    [Test]
    public void Propagated()
    {
        Assert.ThrowsAsync<PropagatedException>(async () =>
        {
            await XceptoTest.Given(new SimpleScenario(), builder =>
            {
                builder.RegisterAdapter(new PropagatedAdapter());
            });
        });
    }
}