using LoggerDisposalTests.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Xcepto.Adapters;
using Xcepto.Interfaces;

namespace LoggerDisposalTests.Adapters;

public class InitExceptionAdapter: XceptoAdapter
{
    private string _message;

    public InitExceptionAdapter(string message)
    {
        _message = message;
    }

    protected override Task Initialize(IServiceProvider serviceProvider)
    {
        var loggingProvider = serviceProvider.GetRequiredService<ILoggingProvider>();
        loggingProvider.LogDebug(_message);
        throw new DisposalInvokingException();
    }
}