using LoggerDisposalTests.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Xcepto.Adapters;
using Xcepto.Interfaces;

namespace LoggerDisposalTests.Adapters;

public class CleanupExceptionAdapter: XceptoAdapter
{
    private string _message;

    public CleanupExceptionAdapter(string message)
    {
        _message = message;
    }

    protected override Task Cleanup(IServiceProvider serviceProvider)
    {
        var loggingProvider = serviceProvider.GetRequiredService<ILoggingProvider>();
        loggingProvider.LogDebug(_message);
        throw new DisposalInvokingException();
    }
}