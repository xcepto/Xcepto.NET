using Xcepto.Data;
using Xcepto.States;

namespace Samples.ExceptionDetail.Tests.States;

public class LongFailingTransitionState: XceptoState
{
    public LongFailingTransitionState(string name) : base(name) { }

    public override async Task<bool> EvaluateConditionsForTransition(IServiceProvider serviceProvider)
    {
        MostRecentFailingResult = new ConditionResult(new { }, "big problem!");
        await Task.Delay(TimeSpan.FromSeconds(5));
        return true;
    }

    public override Task OnEnter(IServiceProvider serviceProvider) => Task.CompletedTask;
}