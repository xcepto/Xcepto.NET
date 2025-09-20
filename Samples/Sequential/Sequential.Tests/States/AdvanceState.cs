using Microsoft.Extensions.DependencyInjection;
using Sequential.Tests.Services;
using Xcepto;

namespace Sequential.Tests.States;

public class AdvanceState: XceptoState
{
    public AdvanceState(string name) : base(name)
    {
    }


    public override Task<bool> EvaluateConditionsForTransition(IServiceProvider serviceProvider) => Task.FromResult(true);

    public override Task OnEnter(IServiceProvider serviceProvider)
    {
        var statefulService = serviceProvider.GetRequiredService<StatefulService>();
        statefulService.Advance();
        return Task.CompletedTask;
    }
}