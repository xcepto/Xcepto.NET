using LoggerDisposalTests.Exceptions;
using LoggerDisposalTests.Provider;
using LoggerDisposalTests.Scenarios;
using LoggerDisposalTests.States;
using Xcepto;

namespace LoggerDisposalTests;

[TestFixture]
public class StateLoggerDisposalTests
{
    [Test]
    public void StateInitDisposal()
    {
        string message = Guid.NewGuid().ToString();
        MockedLoggingProvider mockedLoggingProvider = new MockedLoggingProvider();
        var givenTask = XceptoTest.Given(new BareMockedScenario(mockedLoggingProvider), builder =>
        {
            builder.AddStep(new InitExceptionState("test state init disposal", message));
        });
        Assert.ThrowsAsync<DisposalInvokingException>(() => givenTask);
        Assert.That(mockedLoggingProvider.Flushed(message));
    }
    
    [Test]
    public void StateTransitionDisposal()
    {
        string message = Guid.NewGuid().ToString();
        MockedLoggingProvider mockedLoggingProvider = new MockedLoggingProvider();
        var givenTask = XceptoTest.Given(new BareMockedScenario(mockedLoggingProvider), builder =>
        {
            builder.AddStep(new TransitionExceptionState("test state init disposal", message));
        });
        Assert.ThrowsAsync<DisposalInvokingException>(() => givenTask);
        Assert.That(mockedLoggingProvider.Flushed(message));
    }
    
    [Test]
    public void StateEnterDisposal()
    {
        string message = Guid.NewGuid().ToString();
        MockedLoggingProvider mockedLoggingProvider = new MockedLoggingProvider();
        var givenTask = XceptoTest.Given(new BareMockedScenario(mockedLoggingProvider), builder =>
        {
            builder.AddStep(new EnterExceptionState("test state init disposal", message));
        });
        Assert.ThrowsAsync<DisposalInvokingException>(() => givenTask);
        Assert.That(mockedLoggingProvider.Flushed(message));
    }
}