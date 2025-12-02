using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Xcepto.Builder;
using Xcepto.Scenarios;
using Xcepto.Strategies.Execution;
using Xcepto.TestRunner;

namespace Xcepto
{
    public class XceptoTest
    {
        private IExecutionStrategy _executionStrategy;

        public XceptoTest(IExecutionStrategy executionStrategy)
        {
            _executionStrategy = executionStrategy;
        }
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

        public static async Task Given(XceptoScenario scenario, Action<TransitionBuilder> builder)
            => await Given(scenario, DefaultTimeout, builder);
        public static Task Given(XceptoScenario scenario, TimeSpan timeout,
            Action<TransitionBuilder> builder)
        {
            var xceptoTest = new XceptoTest(new AsyncExecutionStrategy());
            return xceptoTest.GivenWithStrategies(scenario, timeout, builder);
        }

        public Task GivenWithStrategies(XceptoScenario scenario, Action<TransitionBuilder> builder) =>
            GivenWithStrategies(scenario, DefaultTimeout, builder);
        public Task GivenWithStrategies(XceptoScenario scenario, TimeSpan timeout, Action<TransitionBuilder> builder)
        {
            XceptoTestRunner testRunner = new XceptoTestRunner(
                _executionStrategy
            );
            testRunner.Given(scenario, timeout, builder);
            
            if(_executionStrategy is AsyncExecutionStrategy asyncExecutionStrategy)
                return asyncExecutionStrategy.RunAsync();
            if (_executionStrategy is EnumeratedExecutionStrategy enumeratedExecutionStrategy)
                return Task.Run(() => EnumeratedTestRunner.RunEnumerator(enumeratedExecutionStrategy.RunEnumerated()));
            
            throw new ArgumentException("Unknown execution strategy");
        }
        
        public static IEnumerator GivenEnumerated(XceptoScenario scenario, TimeSpan timeout, Action<TransitionBuilder> builder)
        {
            var enumeratedExecutionStrategy = new EnumeratedExecutionStrategy();
            var xceptoTest = new XceptoTest(enumeratedExecutionStrategy);
            return xceptoTest.GivenEnumeratedWithStrategies(scenario, timeout, builder);
        }

        public static IEnumerator GivenEnumerated(XceptoScenario scenario, Action<TransitionBuilder> builder) => 
            GivenEnumerated(scenario, DefaultTimeout, builder);
        
        public IEnumerator GivenEnumeratedWithStrategies(XceptoScenario scenario, Action<TransitionBuilder> builder) =>
            GivenEnumeratedWithStrategies(scenario, DefaultTimeout, builder);
        public IEnumerator GivenEnumeratedWithStrategies(XceptoScenario scenario, TimeSpan timeout, Action<TransitionBuilder> builder)
        {
            if (_executionStrategy is not EnumeratedExecutionStrategy enumeratedExecutionStrategy)
                throw new ArgumentException("Only enumerated strategy allowed");
            
            XceptoTestRunner testRunner = new XceptoTestRunner(
                _executionStrategy
            );
            testRunner.Given(scenario, timeout, builder);

            return enumeratedExecutionStrategy.RunEnumerated();
        }
    }
}