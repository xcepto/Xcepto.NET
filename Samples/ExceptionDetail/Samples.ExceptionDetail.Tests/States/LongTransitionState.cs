using Xcepto.States;

namespace Samples.ExceptionDetail.Tests.States;

public class LongTransitionState: XceptoState
{
    public LongTransitionState(string name) : base(name) { }

    public override async Task<bool> EvaluateConditionsForTransition(IServiceProvider serviceProvider)
    {
        await Task.Delay(TimeSpan.FromSeconds(5));
        return true;
    }

    public override Task OnEnter(IServiceProvider serviceProvider) => Task.CompletedTask;
}