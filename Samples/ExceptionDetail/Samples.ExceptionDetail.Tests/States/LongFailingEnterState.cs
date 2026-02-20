using Xcepto.Data;
using Xcepto.States;

namespace Samples.ExceptionDetail.Tests.States;

public class LongFailingEnterState: XceptoState
{
    public LongFailingEnterState(string name) : base(name) { }

    public override Task<bool> EvaluateConditionsForTransition(IServiceProvider serviceProvider)
    {
        return Task.FromResult(true);
    }

    public override async Task OnEnter(IServiceProvider serviceProvider)
    {
        MostRecentFailingResult = new ConditionResult(new { }, "big problem!");
        await Task.Delay(TimeSpan.FromSeconds(5));
    }
}