using Microsoft.Extensions.DependencyInjection;
using Samples.SSR.GUI.Tests.Services;
using Xcepto.Builder;
using Xcepto.Data;
using Xcepto.Interfaces;
using Xcepto.Provider;
using Xcepto.Scenarios;

namespace Samples.SSR.GUI.Tests.Scenarios;

public class SsrGuiScenario: XceptoScenario
{
    public EnvironmentRuntimeService RuntimeService { get; private set; }

    protected override ScenarioSetup Setup(ScenarioSetupBuilder builder) => builder
        .AddServices(
            x=>x.AddSingleton<ILoggingProvider, XceptoBasicLoggingProvider>()
                .AddSingleton<EnvironmentRuntimeService>()
        )
        .Build();

    protected override ScenarioInitialization Initialize(ScenarioInitializationBuilder builder)
    {
        builder.Do(async x =>
        {
            RuntimeService = x.GetRequiredService<EnvironmentRuntimeService>();
            await RuntimeService.Start();
        });
        
        return base.Initialize(builder);
    }
}