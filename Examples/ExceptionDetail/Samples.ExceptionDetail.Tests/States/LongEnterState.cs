using Xcepto.States;

namespace Samples.ExceptionDetail.Tests.States;

public class LongEnterState: XceptoState
{
    public LongEnterState(string name) : base(name) { }

    public override async Task<bool> EvaluateConditionsForTransition(IServiceProvider serviceProvider)
    {
        return true;
    }

    public override async Task OnEnter(IServiceProvider serviceProvider)
    {
        await Task.Delay(TimeSpan.FromSeconds(5));
    }
}