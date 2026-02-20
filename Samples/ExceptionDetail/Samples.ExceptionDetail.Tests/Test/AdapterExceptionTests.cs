using Samples.ExceptionDetail.Tests.Adapters;
using Samples.ExceptionDetail.Tests.Scenarios;
using Xcepto;
using Xcepto.Exceptions;
using Xcepto.Strategies;
using Xcepto.Strategies.Execution;

namespace Samples.ExceptionDetail.Tests.Test
{
    [TestFixtureSource(typeof(StrategyCombinations), nameof(StrategyCombinations.AllCombinations))]
    public class AdapterExceptionTests
    {
        private XceptoTest _xceptoTest;
        public AdapterExceptionTests(BaseExecutionStrategy executionStrategy)
        {
            _xceptoTest = new XceptoTest(executionStrategy);
        }
            
        [Test]
        public void Failing_Setup()
        {
            Assert.ThrowsAsync<ArrangeTestException>(async () =>
            {
                await _xceptoTest.GivenWithStrategies(new CleanScenario(), builder =>
                {
                    _ = builder.RegisterAdapter(new FailingConstructionAdapter());
                });
            });
        }
        
        [Test]
        public void Failing_Init()
        {
            Assert.ThrowsAsync<AdapterInitException>(async () =>
            {
                await _xceptoTest.GivenWithStrategies(new CleanScenario(), builder =>
                {
                    _ = builder.RegisterAdapter(new FailingInitAdapter());
                });
            });
        }
        
        [Test]
        public void Failing_Cleanup()
        {
            Assert.ThrowsAsync<AdapterCleanupException>(async () =>
            {
                await _xceptoTest.GivenWithStrategies(new CleanScenario(), builder =>
                {
                    _ = builder.RegisterAdapter(new FailingCleanupAdapter());
                });
            });
        }
    }
}