using Sequential.Tests.Services;
using Sequential.Tests.States;
using Xcepto;

namespace Sequential.Tests.Adapter;

public class StatefulAdapter: XceptoAdapter
{
    public void AdvanceState()
    {
        AddStep(new AdvanceState(nameof(AdvanceState)));
    }

    public void ExpectState(State state)
    {
        AddStep(new CheckState($"Expect State {state}", state));
    }
}