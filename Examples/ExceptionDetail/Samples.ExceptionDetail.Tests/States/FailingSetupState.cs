using Xcepto.States;

namespace Samples.ExceptionDetail.Tests.States;

public class FailingSetupState: XceptoState
{
    public FailingSetupState(string name) : base(name)
    {
        throw new Exception();
    }

    public override Task<bool> EvaluateConditionsForTransition(IServiceProvider serviceProvider) =>
        Task.FromResult(true);

    public override Task OnEnter(IServiceProvider serviceProvider) => Task.CompletedTask;
}