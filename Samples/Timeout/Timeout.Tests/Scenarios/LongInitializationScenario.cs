using Microsoft.Extensions.DependencyInjection;
using Timeout.Tests.Providers;
using Xcepto;
using Xcepto.Builder;
using Xcepto.Data;
using Xcepto.Interfaces;
using Xcepto.Internal;
using Xcepto.Scenarios;

namespace Timeout.Tests.Scenarios;

public class LongInitializationScenario: XceptoScenario
{
    protected override ScenarioSetup Setup(ScenarioSetupBuilder builder) => builder
        .AddServices(services => services
            .AddSingleton<ILoggingProvider, LoggingProvider>()
        )
        .Build();

    protected override ScenarioInitialization Initialize(ScenarioInitializationBuilder builder) => builder
        .Do(_ => Task.Delay(TimeSpan.FromSeconds(2)))
        .Build();
}