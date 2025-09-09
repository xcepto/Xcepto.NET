using System;
using System.Collections;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace Xcepto
{
    public static class XceptoTest
    {
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);
        private static readonly TimeSpan TimeoutShutdownTolerance = TimeSpan.FromSeconds(2);

        public static async Task Given(XceptoScenario scenario, TimeSpan timeout,
            Action<TransitionBuilder> builder)
        {
            var transitionBuilder = new TransitionBuilder();
            scenario.AssignBuilder(transitionBuilder);
            builder(transitionBuilder);

            AsyncAcceptanceTest runner = new AsyncAcceptanceTest(timeout, transitionBuilder, scenario);
            var task = runner.ExecuteTestAsync();
            var delayTime = timeout + TimeoutShutdownTolerance;
            var finished = await Task.WhenAny(task, Task.Delay(delayTime));
            
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
                var timeoutException = new TimeoutException($"Test exceeded {delayTime.Seconds} seconds (timeout + tolerance).");
                Console.WriteLine(timeoutException);
                throw timeoutException;
            }
            await task;
        }

        public static async Task Given(XceptoScenario scenario, Action<TransitionBuilder> builder)
            => await Given(scenario, DefaultTimeout, builder);
        
        public static IEnumerator GivenEnumerated(EnumeratedScenario scenario, TimeSpan timeout, Func<TimeSpan, TransitionBuilder, EnumeratedScenario, EnumeratedAcceptanceTest> acceptanceTestSupplier,
            Action<TransitionBuilder> builder)
        {
            var transitionBuilder = new TransitionBuilder();
            builder(transitionBuilder);
        
            EnumeratedAcceptanceTest runner = acceptanceTestSupplier(timeout, transitionBuilder, scenario);
            yield return runner.ExecuteTestEnumerated();
        }

        public static IEnumerator GivenEnumerated(EnumeratedScenario scenario,
            Func<TimeSpan, TransitionBuilder, EnumeratedScenario, EnumeratedAcceptanceTest> acceptanceTestSupplier,
            Action<TransitionBuilder> builder)
        {
            yield return GivenEnumerated(scenario, DefaultTimeout, acceptanceTestSupplier, builder);
        }
    }
}