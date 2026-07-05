using Xcepto;
using Xcepto.States;

namespace Timeout.Tests.States;

public class LongOnEnterState: XceptoState
{
    public LongOnEnterState(string name) : base(name) { }
    public override Task<bool> EvaluateConditionsForTransition(IServiceProvider serviceProvider) 
        => Task.FromResult(true);
    public override Task OnEnter(IServiceProvider serviceProvider) => Task.Delay(TimeSpan.FromSeconds(10));
}