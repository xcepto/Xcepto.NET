using LoggerDisposalTests.Exceptions;
using LoggerDisposalTests.Provider;
using LoggerDisposalTests.Scenarios;
using Xcepto;

namespace LoggerDisposalTests;

[TestFixture]
public class ScenarioLoggerDisposalTests
{
    [Test]
    public void InitDisposal()
    {
        string message = Guid.NewGuid().ToString();
        MockedLoggingProvider mockedLoggingProvider = new MockedLoggingProvider();
        
        var task = XceptoTest.Given(new InitExceptionScenario(mockedLoggingProvider, message), _ => { });
        Assert.ThrowsAsync<DisposalInvokingException>(() => task);
        
        
        Assert.That(mockedLoggingProvider.Flushed(message));
    }
    
    [Test]
    public void CleanupDisposal()
    {
        string message = Guid.NewGuid().ToString();
        MockedLoggingProvider mockedLoggingProvider = new MockedLoggingProvider();
        
        var task = XceptoTest.Given(new CleanupExceptionScenario(mockedLoggingProvider, message), _ => { });
        Assert.ThrowsAsync<DisposalInvokingException>(() => task);
        
        
        Assert.That(mockedLoggingProvider.Flushed(message));
    }
}