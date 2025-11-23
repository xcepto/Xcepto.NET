using Compartments.Tests.Adapters;
using Compartments.Tests.Dependencies;
using Compartments.Tests.Scenarios;
using Compartments.Tests.Service;
using Microsoft.Extensions.DependencyInjection;
using Xcepto;
using Xcepto.Adapters;

namespace Compartments.Tests.Test;

public class AdapterCompartmentAccessTests
{
    
    [Test]
    public async Task PersonalDependenciesAreDifferent()
    {
        await XceptoTest.Given(new CompartmentalizationScenario(), TimeSpan.FromSeconds(3), builder =>
        {
            var compartmentAccessAdapter = builder.RegisterAdapter(new CompartmentAccessAdapter());

            compartmentAccessAdapter.JoinedCompartmentExpectation("service1", "service2",
                (compartment1, compartment2) =>
                {
                    var personalDependency1 = compartment1.Services.GetRequiredService<PersonalDependency>();
                    var personalDependency2 = compartment2.Services.GetRequiredService<PersonalDependency>();

                    return !personalDependency1.Equals(personalDependency2);
                });
        });
    }
    
    [Test]
    public async Task SharedDependenciesAreIdentical()
    {
        await XceptoTest.Given(new CompartmentalizationScenario(), TimeSpan.FromSeconds(3), builder =>
        {
            var compartmentAccessAdapter = builder.RegisterAdapter(new CompartmentAccessAdapter());

            compartmentAccessAdapter.JoinedCompartmentExpectation("service1", "service2",
                (compartment1, compartment2) =>
                {
                    var sharedDependency1 = compartment1.Services.GetRequiredService<SharedDependency>();
                    var sharedDependency2 = compartment2.Services.GetRequiredService<SharedDependency>();

                    return sharedDependency1.Equals(sharedDependency2);
                });
        });
    }
    
    [Test]
    public void CompartmentRepositoryDoesNotExist()
    {
        Assert.CatchAsync<InvalidOperationException>(async () =>
        {
            await XceptoTest.Given(new SharedScenario(), TimeSpan.FromSeconds(3), builder =>
            {
                var compartmentAccessAdapter = builder.RegisterAdapter(new CompartmentAccessAdapter());
                compartmentAccessAdapter.JoinedCompartmentExpectation("service1", "service2",
                    (_, _) => true);
            });
        });
    }

    
}