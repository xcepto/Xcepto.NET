using Microsoft.Extensions.DependencyInjection;
using Sequential.Tests.Adapter;
using Sequential.Tests.Scenario;
using Sequential.Tests.Services;
using Xcepto;

namespace Sequential.Tests.Test;

[TestFixture]
public class SingleFixture
{
    private SequentialTestScenario _scenario;

    [OneTimeSetUp]
    public void Setup()
    {
        _scenario = new SequentialTestScenario();
    }

    [OneTimeTearDown]
    public async Task Teardown()
    {
        await _scenario.TearDown();
    }

    [Test]
    [Order(1)]
    public async Task FirstBringSystemIntoStateA()
    {
        await XceptoTest.Given(_scenario, builder =>
        {
            var statefulAdapter = builder.RegisterAdapter(new StatefulAdapter());

            statefulAdapter.ExpectState(State.A);
        });
    }
    
    [Test]
    [Order(2)]
    public async Task ThenAdvanceSystemToStateB()
    {
        await XceptoTest.Given(_scenario, builder =>
        {
            var statefulAdapter = builder.RegisterAdapter(new StatefulAdapter());

            statefulAdapter.ExpectState(State.A);
            statefulAdapter.AdvanceState();
            statefulAdapter.ExpectState(State.B);
        });
    }
    
    [Test]
    [Order(3)]
    public async Task FinallyAdvanceSystemToStateC()
    {
        await XceptoTest.Given(_scenario, builder =>
        {
            var statefulAdapter = builder.RegisterAdapter(new StatefulAdapter());

            statefulAdapter.ExpectState(State.B);
            statefulAdapter.AdvanceState();
            statefulAdapter.ExpectState(State.C);
        });
    }
}