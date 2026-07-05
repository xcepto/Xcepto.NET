using Samples.ExceptionDetail.Tests.Scenarios;
using Xcepto;
using Xcepto.Exceptions;
using Xcepto.Strategies;
using Xcepto.Strategies.Execution;

namespace Samples.ExceptionDetail.Tests.Test
{
    [TestFixtureSource(typeof(StrategyCombinations), nameof(StrategyCombinations.AllCombinations))]
    public class ScenarioExceptionTests
    {
        private XceptoTest _xceptoTest;
        public ScenarioExceptionTests(BaseExecutionStrategy executionStrategy)
        {
            _xceptoTest = new XceptoTest(executionStrategy);
        }

                            
        [Test]
        public void TestCatchInner()
        {
            Assert.That(async () =>
            {
                await _xceptoTest.GivenWithStrategies(new FailingCleanupScenario(), _ => { });
            }, Throws.Exception.InnerException.TypeOf<IOException>());
        }
        
            
        [Test]
        public void Failing_Setup()
        {
            Assert.ThrowsAsync<ScenarioSetupException>(async () =>
            {
                await _xceptoTest.GivenWithStrategies(new FailingSetupScenario(), _ => { });
            });
        }
        
        [Test]
        public void Failing_Init()
        {
            Assert.ThrowsAsync<ScenarioInitException>(async () =>
            {
                await _xceptoTest.GivenWithStrategies(new FailingInitScenario(), _ => { });
            });
        }
        
        [Test]
        public void Failing_Cleanup()
        {
            Assert.ThrowsAsync<ScenarioCleanupException>(async () =>
            {
                await _xceptoTest.GivenWithStrategies(new FailingCleanupScenario(), _ => { });
            });
        }
    }
}