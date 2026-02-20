using Samples.ExceptionDetail.Tests.Scenarios;
using Xcepto;
using Xcepto.Strategies;
using Xcepto.Strategies.Execution;

namespace Samples.ExceptionDetail.Tests.Test
{
    [TestFixtureSource(typeof(StrategyCombinations), nameof(StrategyCombinations.AllCombinations))]
    public class PreserveInnerExceptionTests
    {
        private XceptoTest _xceptoTest;
        public PreserveInnerExceptionTests(BaseExecutionStrategy executionStrategy)
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
    }
}