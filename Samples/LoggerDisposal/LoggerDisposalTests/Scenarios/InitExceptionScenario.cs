using LoggerDisposalTests.Exceptions;
using LoggerDisposalTests.Provider;
using Microsoft.Extensions.DependencyInjection;
using Xcepto.Builder;
using Xcepto.Data;
using Xcepto.Interfaces;
using Xcepto.Scenarios;

namespace LoggerDisposalTests.Scenarios;

public class InitExceptionScenario: XceptoScenario
{
    private ILoggingProvider _mockedLoggingProvider;
    private string _message;

    public InitExceptionScenario(ILoggingProvider mockedLoggingProvider, string message)
    {
        _message = message;
        _mockedLoggingProvider = mockedLoggingProvider;
    }
    protected override ScenarioSetup Setup(ScenarioSetupBuilder builder)
    {
        return builder
            .AddServices(x => x.AddSingleton<ILoggingProvider>(_mockedLoggingProvider))
            .Build();
    }

    protected override ScenarioInitialization Initialize(ScenarioInitializationBuilder builder)
    {
        builder.Do(x =>
        {
            var loggingProvider = x.GetRequiredService<ILoggingProvider>();
            loggingProvider.LogDebug(_message);
            throw new DisposalInvokingException();
        });
        return base.Initialize(builder);
    }
}