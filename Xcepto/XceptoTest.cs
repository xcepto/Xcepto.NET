using System;
using System.Collections;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using EnumeratedExecutionTests;
using Xcepto.Strategies.Execution;
using Xcepto.Strategies.Isolation;
using Xcepto.Strategies.Scheduling;

namespace Xcepto
{
    public class XceptoTest
    {
        private IExecutionStrategy _executionStrategy;
        private IIsolationStrategy _isolationStrategy;
        private ISchedulingStrategy _schedulingStrategy;

        public XceptoTest(IExecutionStrategy executionStrategy, IIsolationStrategy isolationStrategy, ISchedulingStrategy schedulingStrategy)
        {
            _schedulingStrategy = schedulingStrategy;
            _isolationStrategy = isolationStrategy;
            _executionStrategy = executionStrategy;
        }
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

        public static Task Given(BaseScenario scenario, TimeSpan timeout,
            Action<TransitionBuilder> builder)
        {
            var xceptoTest = new XceptoTest(new AsyncExecutionStrategy(), new NoIsolationStrategy(), new ParallelSchedulingStrategy());
            return xceptoTest.GivenWithStrategies(scenario, timeout, builder);
        }

        public Task GivenWithStrategies(BaseScenario scenario, Action<TransitionBuilder> builder) =>
            GivenWithStrategies(scenario, DefaultTimeout, builder);
        public Task GivenWithStrategies(BaseScenario scenario, TimeSpan timeout, Action<TransitionBuilder> builder)
        {
            XceptoTestRunner testRunner = new XceptoTestRunner(
                _executionStrategy,
                _schedulingStrategy,
                _isolationStrategy
            );
            testRunner.Given(scenario, timeout, builder);
            
            if(_executionStrategy is AsyncExecutionStrategy asyncExecutionStrategy)
                return asyncExecutionStrategy.RunAsync();
            if (_executionStrategy is EnumeratedExecutionStrategy enumeratedExecutionStrategy)
                return Task.Run(() => EnumeratedTestRunner.RunEnumerator(enumeratedExecutionStrategy.RunEnumerated()));
            
            throw new ArgumentException("Unknown execution strategy");
        }
        
        public static async Task GivenSequential(BaseScenario scenario, TimeSpan timeout,
            Action<TransitionBuilder> builder)
        {
            var transitionBuilder = new TransitionBuilder();
            scenario.AssignBuilder(transitionBuilder);
            builder(transitionBuilder);

            await RunAsync(scenario, timeout, transitionBuilder);
        }

        private static async Task RunAsync(BaseScenario scenario, TimeSpan timeout, TransitionBuilder transitionBuilder)
        {
            AsyncAcceptanceTest runner = new AsyncAcceptanceTest(timeout, transitionBuilder, scenario);
            var task = runner.ExecuteTestAsync();
            var finished = await Task.WhenAny(task, Task.Delay(timeout));
            
            // Log all exceptions
            foreach (var propagatedTask in transitionBuilder.PropagatedTasks)
            {
                if(propagatedTask.IsFaulted && propagatedTask.Exception is not null)
                    Console.WriteLine(propagatedTask.Exception);
            }

            // throw first exception
            var firstException = transitionBuilder.PropagatedTasks
                .FirstOrDefault(x => x.IsFaulted && x.Exception is not null);
            if (firstException?.Exception is not null)
            {
                var inner = firstException.Exception.InnerException ?? firstException.Exception;
                ExceptionDispatchInfo.Capture(inner).Throw();
            }
            
            if (finished != task)
            {
                var timeoutException = new TimeoutException($"Test exceeded {timeout.Seconds} seconds (timeout).");
                Console.WriteLine(timeoutException);
                throw timeoutException;
            }
            await task;
        }

        public static async Task Given(BaseScenario scenario, Action<TransitionBuilder> builder)
            => await Given(scenario, DefaultTimeout, builder);
        
        public static IEnumerator GivenEnumerated(BaseScenario scenario, TimeSpan timeout, Func<TimeSpan, TransitionBuilder, BaseScenario, EnumeratedAcceptanceTest> acceptanceTestSupplier,
            Action<TransitionBuilder> builder)
        {
            var transitionBuilder = new TransitionBuilder();
            builder(transitionBuilder);
        
            EnumeratedAcceptanceTest runner = acceptanceTestSupplier(timeout, transitionBuilder, scenario);
            yield return runner.ExecuteTestEnumerated();
        }

        public static IEnumerator GivenEnumerated(BaseScenario scenario,
            Func<TimeSpan, TransitionBuilder, BaseScenario, EnumeratedAcceptanceTest> acceptanceTestSupplier,
            Action<TransitionBuilder> builder)
        {
            yield return GivenEnumerated(scenario, DefaultTimeout, acceptanceTestSupplier, builder);
        }
    }
}