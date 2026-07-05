using Xcepto.States;

namespace Samples.ExceptionDetail.Tests.States;

public class FailingEnterState: XceptoState
{
    public FailingEnterState(string name) : base(name) { }

    public override Task<bool> EvaluateConditionsForTransition(IServiceProvider serviceProvider) =>
        Task.FromResult(true);

    public override Task OnEnter(IServiceProvider serviceProvider)
    {
        throw new Exception();
    }
}