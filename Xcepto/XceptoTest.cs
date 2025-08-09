using System;
using System.Collections;
using System.Threading.Tasks;

namespace Xcepto
{
    public static class XceptoTest
    {
        public static async Task Given(Scenario scenario, TimeSpan timeout,
            Action<TransitionBuilder> builder)
        {
            var transitionBuilder = new TransitionBuilder();
            builder(transitionBuilder);
        
            AsyncAcceptanceTest runner = new AsyncAcceptanceTest(timeout, transitionBuilder, scenario);
            await runner.ExecuteTestAsync();
        }

        public static async Task Given(Scenario scenario, Action<TransitionBuilder> builder)
            => await Given(scenario, TimeSpan.FromSeconds(5), builder);
        
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
            yield return GivenEnumerated(scenario, TimeSpan.FromSeconds(5), acceptanceTestSupplier, builder);
        }
    }
}