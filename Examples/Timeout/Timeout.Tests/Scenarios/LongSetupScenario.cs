using Microsoft.Extensions.DependencyInjection;
using Xcepto;
using Xcepto.Builder;
using Xcepto.Data;
using Xcepto.Interfaces;
using Xcepto.Provider;
using Xcepto.Scenarios;

namespace Timeout.Tests.Scenarios;

public class LongSetupScenario: XceptoScenario
{
    protected override ScenarioSetup Setup(ScenarioSetupBuilder builder) => builder
        .Do(() => Task.Delay(TimeSpan.FromSeconds(10)))
        .AddServices(services => services
            .AddSingleton<ILoggingProvider, XceptoBasicLoggingProvider>()
        )
        .Build();
    
    
}