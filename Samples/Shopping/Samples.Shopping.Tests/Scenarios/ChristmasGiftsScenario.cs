using Microsoft.Extensions.DependencyInjection;
using Samples.Shopping.Tests.providers;
using Xcepto;
using Xcepto.Interfaces;

namespace Samples.Shopping.Tests.scenarios;

public class ChristmasGiftsScenario: Scenario
{
    public override IServiceCollection Setup()
    {
        LoggingProvider loggingProvider = new LoggingProvider();
        
        ServiceCollection services = new ServiceCollection();
        services.AddSingleton<ILoggingProvider, LoggingProvider>(x => loggingProvider);
        return services;
    }
}