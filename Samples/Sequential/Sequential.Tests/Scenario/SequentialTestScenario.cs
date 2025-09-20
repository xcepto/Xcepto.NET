using Microsoft.Extensions.DependencyInjection;
using Sequential.Tests.Services;
using Xcepto;
using Xcepto.Interfaces;
using Xcepto.Provider;

namespace Sequential.Tests.Scenario;

public class SequentialTestScenario: SequentialScenario
{
    protected override async Task<IServiceCollection> OneTimeSetup()
    {
        var services = await base.OneTimeSetup();
        services.AddSingleton<ILoggingProvider, XceptoBasicLoggingProvider>();
        services.AddSingleton<StatefulService>();
        return services;
    }
}