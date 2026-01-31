using Microsoft.Extensions.DependencyInjection;
using Xcepto;
using Xcepto.Builder;
using Xcepto.Data;
using Xcepto.Interfaces;
using Xcepto.Provider;
using Xcepto.Scenarios;

namespace ExceptionPropagation.Tests.Scenario;

public class SimpleSyncScenario: XceptoScenario
{
    protected override ScenarioSetup Setup(ScenarioSetupBuilder builder) => builder
        .AddServices(services => services
            .AddSingleton<ILoggingProvider, XceptoBasicLoggingProvider>()
        )
        .Build();
}