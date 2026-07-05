using LoggerDisposalTests.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Xcepto.Interfaces;
using Xcepto.States;

namespace LoggerDisposalTests.States;

public class InitExceptionState: XceptoState
{
    private string _message;
    public InitExceptionState(string name, string message) : base(name)
    {
        _message = message;
    }

    public override Task Initialize(IServiceProvider serviceProvider)
    {
        var loggingProvider = serviceProvider.GetRequiredService<ILoggingProvider>();
        loggingProvider.LogDebug(_message);
        throw new DisposalInvokingException();
    }

    public override Task<bool> EvaluateConditionsForTransition(IServiceProvider serviceProvider) =>
        Task.FromResult(false);
    public override Task OnEnter(IServiceProvider serviceProvider) => Task.CompletedTask;
}