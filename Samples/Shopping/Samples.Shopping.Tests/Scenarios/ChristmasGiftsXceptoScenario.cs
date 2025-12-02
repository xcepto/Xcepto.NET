using Microsoft.Extensions.DependencyInjection;
using Samples.Shopping.Tests.Providers;
using Xcepto;
using Xcepto.Interfaces;
using Xcepto.RabbitMQ;
using Xcepto.Scenarios;

namespace Samples.Shopping.Tests.Scenarios;

public class ChristmasGiftsSyncScenario: AsyncScenario
{
    protected override Task<IServiceCollection> Setup()
    {
        LoggingProvider loggingProvider = new LoggingProvider();
        
        ServiceCollection services = new ServiceCollection();
        services.AddSingleton<ILoggingProvider, LoggingProvider>(x => loggingProvider);
        services.AddSingleton<XceptoRabbitMqRepository>();
        return Task.FromResult<IServiceCollection>(services);
    }
}