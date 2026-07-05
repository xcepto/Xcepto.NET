using Microsoft.Extensions.DependencyInjection;
using Sequential.Tests.Services;
using Xcepto;
using Xcepto.States;

namespace Sequential.Tests.States;

public class CheckState: XceptoState
{
    private State _expectedState;

    public CheckState(string name, State expectedState) : base(name)
    {
        _expectedState = expectedState;
    }

    public override Task<bool> EvaluateConditionsForTransition(IServiceProvider serviceProvider)
    {
        var statefulService = serviceProvider.GetRequiredService<StatefulService>();
        return Task.FromResult(statefulService.State == _expectedState);
    }

    public override Task OnEnter(IServiceProvider serviceProvider) => Task.CompletedTask;
}