using Xcepto;
using Xcepto.States;

namespace Timeout.Tests.States;

public class LongInitializeState: XceptoState
{
    public LongInitializeState(string name) : base(name) { }
    public override Task<bool> EvaluateConditionsForTransition(IServiceProvider serviceProvider) 
        => Task.FromResult(true);
    public override Task OnEnter(IServiceProvider serviceProvider)
        => Task.CompletedTask;
    
    public override Task Initialize(IServiceProvider serviceProvider)
        => Task.Delay(TimeSpan.FromSeconds(10));
}