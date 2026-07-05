using Xcepto.States;

namespace Samples.ExceptionDetail.Tests.States;

public class FailingInitState: XceptoState
{
    public FailingInitState(string name) : base(name) { }

    public override Task<bool> EvaluateConditionsForTransition(IServiceProvider serviceProvider) =>
        Task.FromResult(true);

    public override Task OnEnter(IServiceProvider serviceProvider) => Task.CompletedTask;

    public override Task Initialize(IServiceProvider serviceProvider)
    {
        throw new Exception();
    }
}