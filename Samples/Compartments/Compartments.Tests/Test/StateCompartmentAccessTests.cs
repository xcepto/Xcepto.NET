using Compartments.Tests.Adapters;
using Compartments.Tests.Dependencies;
using Compartments.Tests.Scenarios;
using Compartments.Tests.Service;
using Microsoft.Extensions.DependencyInjection;
using Xcepto;
using Xcepto.Config;

namespace Compartments.Tests.Test;

[TestFixture]
public class StateCompartmentAccessTests
{
    [Test]
    public async Task PersonalDependenciesAreDifferent()
    {
        await XceptoTest.Given(new CompartmentalizationScenario(), TimeoutConfig.FromSeconds(3), builder =>
        {
            var compartmentAccessAdapter = builder.RegisterAdapter(new CompartmentAccessAdapter());
            
            compartmentAccessAdapter.CompartmentStepInitializationExpectation("service1", 0);
            compartmentAccessAdapter.CompartmentServiceAction<Service1>("service1", service1 => 
                service1.Act());
            
            // Value fetched during state initialization is still 0
            compartmentAccessAdapter.CompartmentStepInitializationExpectation("service1", 0);
            
            // Value fetched now is 1
            compartmentAccessAdapter.CompartmentExpectation<Service1>("service1", 
                service1 => service1.GetValue().Equals(1));
            
        });
    }
}