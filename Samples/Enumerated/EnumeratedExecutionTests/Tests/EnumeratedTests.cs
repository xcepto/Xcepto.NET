using EnumeratedExecutionTests.Repositories;
using EnumeratedExecutionTests.Scenario;
using EnumeratedExecutionTests.Services;
using Xcepto;
using Xcepto.Adapters;
using Xcepto.TestRunner;

namespace EnumeratedExecutionTests.Tests;

[TestFixture]
public class EnumeratedTests
{
    [Test]
    public void GivenEnumerated()
    {
        var enumerator = XceptoTest.GivenEnumerated(new ExampleScenario(), TimeSpan.FromSeconds(5),
            builder =>
            {
                var serviceAdapter = builder.RegisterAdapter(new GenericServiceAdapter());
                
                serviceAdapter.ServiceExpectation<Repository>(repo => repo.Get() == 0);
                serviceAdapter.ServiceAction<ServiceA>(service => service.Act());
                serviceAdapter.ServiceExpectation<Repository>(repo => repo.Get() == 1);
                serviceAdapter.ServiceAction<ServiceB>(service => service.Act());
                serviceAdapter.ServiceExpectation<Repository>(repo => repo.Get() == 2);
            });

        EnumeratedTestRunner.RunEnumerator(enumerator);
    }
}