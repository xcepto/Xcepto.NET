using Xcepto.States;

namespace Samples.ExceptionDetail.Tests.States;

public class FailingTransitionState: XceptoState
{
    public FailingTransitionState(string name) : base(name) { }

    public override Task<bool> EvaluateConditionsForTransition(IServiceProvider serviceProvider)
    {
        throw new Exception();
    }

    public override Task OnEnter(IServiceProvider serviceProvider) => Task.CompletedTask;
}