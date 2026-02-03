using LoggerDisposalTests.Exceptions;
using LoggerDisposalTests.Provider;
using LoggerDisposalTests.Scenarios;
using Xcepto;
using Xcepto.Builder;
using Xcepto.Config;
using Xcepto.Interfaces;
using Xcepto.Scenarios;
using Xcepto.Strategies.Execution;
using Xcepto.TestRunner;

namespace LoggerDisposalTests;


[TestFixture(typeof(AsyncExecutionStrategy))]
[TestFixture(typeof(EnumeratedExecutionStrategy))]
public class ScenarioLoggerDisposalTests<T>
where T : BaseExecutionStrategy, new()
{
    public ScenarioLoggerDisposalTests()
    {
        _executionStrategy = Activator.CreateInstance<T>();
        _xceptoTestRunner = new XceptoTestRunner(_executionStrategy);
    }
    private string _message = Guid.NewGuid().ToString();
    private XceptoTestRunner _xceptoTestRunner;
    private T _executionStrategy;

    private void Run(Func<ILoggingProvider, string ,XceptoScenario> scenarioBuilder)
    {
        MockedLoggingProvider mockedLoggingProvider = new MockedLoggingProvider();
        _xceptoTestRunner.Given(scenarioBuilder(mockedLoggingProvider, _message), 
            TimeoutConfig.FromSeconds(5), _ => {});
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
        Run((provider, message) => new InitExceptionScenario(provider, message));
    }
    
    [Test]
    public void CleanupDisposal()
    {
        Run((provider, message) => new CleanupExceptionScenario(provider, message));
    }
}