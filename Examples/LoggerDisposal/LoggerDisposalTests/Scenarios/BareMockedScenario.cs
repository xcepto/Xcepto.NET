using LoggerDisposalTests.Exceptions;
using LoggerDisposalTests.Provider;
using Microsoft.Extensions.DependencyInjection;
using Xcepto.Builder;
using Xcepto.Data;
using Xcepto.Interfaces;
using Xcepto.Scenarios;

namespace LoggerDisposalTests.Scenarios;

public class BareMockedScenario: XceptoScenario
{
    private MockedLoggingProvider _mockedLoggingProvider;

    public BareMockedScenario(MockedLoggingProvider mockedLoggingProvider)
    {
        _mockedLoggingProvider = mockedLoggingProvider;
    }
    protected override ScenarioSetup Setup(ScenarioSetupBuilder builder)
    {
        return builder
            .AddServices(x => x.AddSingleton<ILoggingProvider>(_mockedLoggingProvider))
            .Build();
    }
}