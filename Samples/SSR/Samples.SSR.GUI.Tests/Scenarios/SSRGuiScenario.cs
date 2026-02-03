using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Samples.SSR.GUI.Tests.Util;
using Testcontainers.PostgreSql;
using Xcepto.Builder;
using Xcepto.Data;
using Xcepto.Interfaces;
using Xcepto.Provider;
using Xcepto.Scenarios;

namespace Samples.SSR.GUI.Tests.Scenarios;

public class SsrGuiScenario: XceptoScenario
{
    public ushort GuiPort { get; private set; }

    protected override ScenarioSetup Setup(ScenarioSetupBuilder builder) => builder
        .AddServices(x=>x
            .AddSingleton<ILoggingProvider, XceptoBasicLoggingProvider>()
            .AddSingleton<OutputConsumerProvider>()
        )
        .RegisterDisposable<OutputConsumerProvider>()
        .RegisterDisposable<ILoggingProvider>()
        .Build();

    protected override ScenarioInitialization Initialize(ScenarioInitializationBuilder builder)
    {
        builder.Do(StartEnvironment);
        return base.Initialize(builder);
    }

    private async Task StartEnvironment(IServiceProvider serviceProvider)
    {
        var loggingProvider = serviceProvider.GetRequiredService<ILoggingProvider>();
        var outputConsumerProvider = serviceProvider.GetRequiredService<OutputConsumerProvider>();
        var loggerProvider = new LoggerProvider(loggingProvider);
        var loggerFactory = LoggerFactory.Create(loggingBuilder =>
        {
            loggingBuilder
                .SetMinimumLevel(LogLevel.Warning)
                .AddDebug()
                .AddProvider(loggerProvider);
        });
    
        var baseLogger = loggerFactory.CreateLogger<SsrGuiScenario>();
        var filteredLogger = new TestContainersLogFilter(baseLogger);
        
        var network = new NetworkBuilder()
            .WithName("test-network")
            .Build();
        
        var guiImage = new ImageFromDockerfileBuilder()
            .WithName("samples-ssr-gui:test")
            .WithDockerfileDirectory(CommonDirectoryPath.GetSolutionDirectory(), Path.Combine("Samples", "SSR"))
            .WithDockerfile("Samples.SSR.GUI/Dockerfile")
            .WithLogger(filteredLogger)
            .Build();
        await guiImage.CreateAsync();
        
        var _postgres = new PostgreSqlBuilder("postgres:18")
            .WithDatabase("test")
            .WithUsername("test")
            .WithPassword("test")
            .WithPortBinding(5432, true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilCommandIsCompleted("pg_isready -U test -d test"))
            .WithLogger(filteredLogger)
            .WithNetwork(network)
            .WithOutputConsumer(outputConsumerProvider.Create("postgres", false))
            .WithNetworkAliases("postgres")
            .Build();
        await _postgres.StartAsync();
        
        var _gui = new ContainerBuilder(guiImage)
            .WithPortBinding(8080, true)
            .WithEnvironment("POSTGRES_HOST", "postgres")
            .WithEnvironment("POSTGRES_PORT", "5432")
            .WithEnvironment("POSTGRES_USER", "test")
            .WithEnvironment("POSTGRES_PASSWORD", "test")
            .WithEnvironment("POSTGRES_DB", "test")
            .WithOutputConsumer(outputConsumerProvider.Create("gui", true))
            .WithWaitStrategy(Wait.ForUnixContainer()
                .UntilHttpRequestIsSucceeded(x => x
                    .ForPath("/")
                    .ForPort(8080)
                )
            )
            .WithLogger(filteredLogger)
            .WithNetwork(network)
            .Build();
        await _gui.StartAsync();
                
        GuiPort = _gui.GetMappedPublicPort(8080);
    }
}