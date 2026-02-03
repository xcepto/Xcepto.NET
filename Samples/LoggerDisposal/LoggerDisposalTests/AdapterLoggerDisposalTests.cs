using LoggerDisposalTests.Adapters;
using LoggerDisposalTests.Exceptions;
using LoggerDisposalTests.Provider;
using LoggerDisposalTests.Scenarios;
using Xcepto;
using Xcepto.Builder;
using Xcepto.Config;
using Xcepto.Strategies.Execution;
using Xcepto.TestRunner;

namespace LoggerDisposalTests;

[TestFixture(typeof(AsyncExecutionStrategy))]
[TestFixture(typeof(EnumeratedExecutionStrategy))]
public class AdapterLoggerDisposalTests<T>
where T: BaseExecutionStrategy, new()
{
    public AdapterLoggerDisposalTests()
    {
        _executionStrategy = Activator.CreateInstance<T>();
        _xceptoTestRunner = new XceptoTestRunner(_executionStrategy);
    }
    private string _message = Guid.NewGuid().ToString();
    private XceptoTestRunner _xceptoTestRunner;
    private T _executionStrategy;

    private void Run(Action<TransitionBuilder> builder)
    {
        MockedLoggingProvider mockedLoggingProvider = new MockedLoggingProvider();
        _xceptoTestRunner.Given(new BareMockedScenario(mockedLoggingProvider), 
            TimeoutConfig.FromSeconds(5), builder);
        if (_executionStrategy is AsyncExecutionStrategy asyncExecutionStrategy)
        {
            Assert.ThrowsAsync<DisposalInvokingException>(async () =>
            {
                await asyncExecutionStrategy.RunAsync();
            });
        }
        else if (_executionStrategy is EnumeratedExecutionStrategy enumeratedExecutionStrategy)
        {
            Assert.Throws<DisposalInvokingException>(() =>
            {
                EnumeratedTestRunner.RunEnumerator(enumeratedExecutionStrategy.RunEnumerated());   
            });
        }
        
        Assert.That(mockedLoggingProvider.Flushed(_message));
    }
    
    [Test]
    public void InitDisposal()
    {
        Run(builder =>
        {
            builder.RegisterAdapter(new InitExceptionAdapter(_message));
        });
    }
    
    [Test]
    public void CleanupDisposal()
    {
        Run(builder =>
        {
            builder.RegisterAdapter(new CleanupExceptionAdapter(_message));
        });
    }
}