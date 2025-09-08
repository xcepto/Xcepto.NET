using ExceptionPropagation.Tests.Exceptions;
using Xcepto;

namespace ExceptionPropagation.Tests.States;

public class UnpropagatedState: XceptoState
{
    public UnpropagatedState(string name) : base(name)
    {
    }
    public override Task<bool> EvaluateConditionsForTransition(IServiceProvider serviceProvider) =>
        Task.FromResult(true);

    public override Task OnEnter(IServiceProvider serviceProvider)
    {
        Task.Run(() =>
        {
            throw new PropagatedException();
        });
        return Task.CompletedTask;
    }
}