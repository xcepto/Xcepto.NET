using System.Collections;
using Xcepto;

namespace EnumeratedExecutionTests;

public class SimpleEnumeratedAcceptanceTest: EnumeratedAcceptanceTest
{
    private TimeSpan _timeout;

    public SimpleEnumeratedAcceptanceTest(TimeSpan timeout, TransitionBuilder transitionBuilder, EnumeratedScenario enumeratedScenario) : base(timeout, transitionBuilder, enumeratedScenario)
    {
        _timeout = timeout;
    }

    protected override IEnumerator EnumeratedWait()
    {
        var end = DateTime.UtcNow + TimeSpan.FromSeconds(0.1f);

        while (DateTime.UtcNow < end)
            yield return null;
    }
}