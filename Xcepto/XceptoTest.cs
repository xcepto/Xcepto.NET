using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace Xcepto
{
    public static class XceptoTest
    {
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(5);
        private static readonly TimeSpan TimeoutShutdownTolerance = TimeSpan.FromSeconds(2);

        public static async Task Given(Scenario scenario, TimeSpan timeout,
            Action<TransitionBuilder> builder)
        {
            var transitionBuilder = new TransitionBuilder();
            builder(transitionBuilder);

            AsyncAcceptanceTest runner = new AsyncAcceptanceTest(timeout, transitionBuilder, scenario);
            var task = runner.ExecuteTestAsync();
            var delayTime = timeout + TimeoutShutdownTolerance;
            var finished = await Task.WhenAny(task, Task.Delay(delayTime));

            if (finished != task)
                throw new TimeoutException($"Test exceeded {delayTime.Seconds} seconds (timeout + tolerance).");

            await task;
        }

        public static async Task Given(Scenario scenario, Action<TransitionBuilder> builder)
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