using Compartments.Tests.Dependencies;
using Compartments.Tests.Scenarios;
using Compartments.Tests.Service;
using Xcepto;
using Xcepto.Adapters;

namespace Compartments.Tests.Test;

[TestFixture]
public class DifferentServiceTests
{
    private static Action<TransitionBuilder> Definition =>
        builder =>
        {
            var serviceAdapter = builder.RegisterAdapter(new GenericServiceAdapter());
            serviceAdapter.ServiceAction<Service1>(x => x.Act());
            serviceAdapter.ServiceAction<Service2>(x => x.Act());

            serviceAdapter.ServiceExpectation<Service1>(x => x.GetValue() == 1);
            serviceAdapter.ServiceExpectation<Service2>(x => x.GetValue() == 1);
            serviceAdapter.ServiceExpectation<SharedDependency>(x => x.Value() == 2);
        };


    [Test]
    public async Task ServicesDontAffectEachOther()
    {
        await XceptoTest.Given(new CompartmentalizationScenario(), TimeSpan.FromSeconds(3), Definition);
    }
    
    [Test]
    public void ServicesAffectEachOther()
    {
        Assert.CatchAsync<TimeoutException>(async () =>
        {
            await XceptoTest.Given(new SharedScenario(), TimeSpan.FromSeconds(3), Definition);
        });
    }
}