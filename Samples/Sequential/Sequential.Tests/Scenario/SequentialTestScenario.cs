using Microsoft.Extensions.DependencyInjection;
using Sequential.Tests.Services;
using Xcepto;
using Xcepto.Builder;
using Xcepto.Data;
using Xcepto.Interfaces;
using Xcepto.Internal;
using Xcepto.Provider;
using Xcepto.Scenarios;

namespace Sequential.Tests.Scenario;

public class SequentialTestScenario: XceptoScenario
{
    protected override ScenarioSetup Setup(ScenarioSetupBuilder builder) => builder
        .AddServices(services => services
            .AddSingleton<ILoggingProvider, XceptoBasicLoggingProvider>()
            .AddSingleton<StatefulService>()
        )
        .Build();

    protected override ScenarioInitialization Initialize(ScenarioInitializationBuilder builder) => builder.Build();

    protected override ScenarioCleanup Cleanup(ScenarioCleanupBuilder builder) => builder.Build();
}