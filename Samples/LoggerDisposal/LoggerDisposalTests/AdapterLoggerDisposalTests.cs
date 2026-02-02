using LoggerDisposalTests.Adapters;
using LoggerDisposalTests.Exceptions;
using LoggerDisposalTests.Provider;
using LoggerDisposalTests.Scenarios;
using Xcepto;

namespace LoggerDisposalTests;

[TestFixture]
public class AdapterLoggerDisposalTests
{
    [Test]
    public void InitDisposal()
    {
        string message = Guid.NewGuid().ToString();
        MockedLoggingProvider mockedLoggingProvider = new MockedLoggingProvider();
        var givenTask = XceptoTest.Given(new BareMockedScenario(mockedLoggingProvider), builder =>
        {
            builder.RegisterAdapter(new InitExceptionAdapter(message));
        });
        Assert.ThrowsAsync<DisposalInvokingException>(() => givenTask);
        Assert.That(mockedLoggingProvider.Flushed(message));
    }
    
    [Test]
    public void CleanupDisposal()
    {
        string message = Guid.NewGuid().ToString();
        MockedLoggingProvider mockedLoggingProvider = new MockedLoggingProvider();
        var givenTask = XceptoTest.Given(new BareMockedScenario(mockedLoggingProvider), builder =>
        {
            builder.RegisterAdapter(new CleanupExceptionAdapter(message));
        });
        Assert.ThrowsAsync<DisposalInvokingException>(() => givenTask);
        Assert.That(mockedLoggingProvider.Flushed(message));
    }
}