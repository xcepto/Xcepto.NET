using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.DependencyInjection;
using Samples.SSR.GUI.Tests.Providers;
using Testcontainers.PostgreSql;
using Xcepto.Builder;
using Xcepto.Data;
using Xcepto.Interfaces;
using Xcepto.Provider;
using Xcepto.Scenarios;

namespace Samples.SSR.GUI.Tests.Scenarios;

public class SsrGuiScenario: XceptoScenario
{
    protected override ScenarioSetup Setup(ScenarioSetupBuilder builder) => builder
        .AddServices(
            x=>x.AddSingleton<ILoggingProvider, XceptoBasicLoggingProvider>()
        )
        .Build();
    
}