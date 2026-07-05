using Microsoft.Extensions.DependencyInjection;
using Xcepto;
using Xcepto.Builder;
using Xcepto.Data;
using Xcepto.Interfaces;
using Xcepto.Internal;
using Xcepto.Provider;
using Xcepto.Scenarios;

namespace Timeout.Tests.Scenarios;

public class LongCleanupScenario: XceptoScenario
{
    
    protected override ScenarioSetup Setup(ScenarioSetupBuilder builder) => builder
        .AddServices(services => services
            .AddSingleton<ILoggingProvider, XceptoBasicLoggingProvider>()
        )
        .Build();
    
    protected override ScenarioCleanup Cleanup(ScenarioCleanupBuilder builder) => builder
        .Do(async () => await Task.Delay(TimeSpan.FromSeconds(10)))
        .Build();
}