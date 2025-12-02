using EnumeratedExecutionTests.Repositories;
using EnumeratedExecutionTests.Scenario;
using EnumeratedExecutionTests.Services;
using Xcepto;
using Xcepto.Adapters;
using Xcepto.Strategies.Execution;
using Xcepto.TestRunner;

namespace EnumeratedExecutionTests.Tests;

[TestFixture]
public class XceptoTestRunnerTests
{
    [Test]
    public void EnumeratedTestRunnerRunsProperly()
    {
        var enumeratedExecutionStrategy = new EnumeratedExecutionStrategy();
        XceptoTestRunner testRunner = new XceptoTestRunner(enumeratedExecutionStrategy);

        testRunner.Given(new ExampleScenario(), TimeSpan.FromSeconds(5), builder =>
        {
            var serviceAdapter = builder.RegisterAdapter(new GenericServiceAdapter());
                
            serviceAdapter.ServiceExpectation<Repository>(repo => repo.Get() == 0);
            serviceAdapter.ServiceAction<ServiceA>(service => service.Act());
            serviceAdapter.ServiceExpectation<Repository>(repo => repo.Get() == 1);
            serviceAdapter.ServiceAction<ServiceB>(service => service.Act());
            serviceAdapter.ServiceExpectation<Repository>(repo => repo.Get() == 2);
        });
            
            
        EnumeratedTestRunner.RunEnumerator(enumeratedExecutionStrategy.RunEnumerated());
    }
}