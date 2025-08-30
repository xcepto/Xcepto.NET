using Xcepto;

namespace Endless.Tests.States;

public class LongEvaluationState: XceptoState
{
    public LongEvaluationState(string name) : base(name) { }

    public override async Task<bool> EvaluateConditionsForTransition(IServiceProvider serviceProvider)
    {
        await Task.Delay(TimeSpan.FromSeconds(10));
        return true;
    }
    public override Task OnEnter(IServiceProvider serviceProvider) 
        => Task.CompletedTask;
}