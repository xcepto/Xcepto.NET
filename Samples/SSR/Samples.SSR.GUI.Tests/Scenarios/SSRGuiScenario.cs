using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Testcontainers.PostgreSql;
using Xcepto.Builder;
using Xcepto.Data;
using Xcepto.Interfaces;
using Xcepto.Provider;
using Xcepto.Scenarios;
using Xcepto.Testcontainers.Extensions;
using Xcepto.Testcontainers.Interfaces;

namespace Samples.SSR.GUI.Tests.Scenarios;

public class SsrGuiScenario: XceptoScenario
{
    private Guid identifier = Guid.NewGuid();
    private PostgreSqlContainer? _postgresContainer;
    private IContainer? _guiContainer;
    public ushort GuiPort { get; private set; }

    protected override ScenarioSetup Setup(ScenarioSetupBuilder builder) => builder
        .AddTestcontainersSupport()
        .Build();

    protected override ScenarioInitialization Initialize(ScenarioInitializationBuilder builder)
    {
        builder.Do(StartEnvironment);
        return base.Initialize(builder);
    }

    private async Task StartEnvironment(IServiceProvider serviceProvider)
    {
        var testContainerSupport = serviceProvider.GetRequiredService<ITestContainerSupport>();
        
        var network = new NetworkBuilder()
            .WithName($"test-network-{identifier.ToString()}")
            .Build();
        
        var guiImage = new ImageFromDockerfileBuilder()
            .WithName("samples-ssr-gui:test")
            .WithDockerfileDirectory(CommonDirectoryPath.GetSolutionDirectory(), Path.Combine("Samples", "SSR"))
            .WithDockerfile("Samples.SSR.GUI/Dockerfile")
            .WithLogger(testContainerSupport.CreateLogger("gui-image-builder"))
            .Build();
        await guiImage.CreateAsync();
        
        _postgresContainer = new PostgreSqlBuilder("postgres:18")
            .WithDatabase("test")
            .WithUsername("test")
            .WithPassword("test")
            .WithPortBinding(5432, true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilCommandIsCompleted("pg_isready -U test -d test"))
            .WithLogger(testContainerSupport.CreateLogger("postres"))
            .WithNetwork(network)
            .WithOutputConsumer(testContainerSupport.CreateOutputConsumer("postgres", false))
            .WithNetworkAliases("postgres")
            .Build();
        await _postgresContainer.StartAsync();
        
        _guiContainer = new ContainerBuilder(guiImage)
            .WithPortBinding(8080, true)
            .WithEnvironment("POSTGRES_HOST", "postgres")
            .WithEnvironment("POSTGRES_PORT", "5432")
            .WithEnvironment("POSTGRES_USER", "test")
            .WithEnvironment("POSTGRES_PASSWORD", "test")
            .WithEnvironment("POSTGRES_DB", "test")
            .WithOutputConsumer(testContainerSupport.CreateOutputConsumer("gui", true))
            .WithWaitStrategy(Wait.ForUnixContainer()
                .UntilHttpRequestIsSucceeded(x => x
                    .ForPath("/")
                    .ForPort(8080)
                )
            )
            .WithLogger(testContainerSupport.CreateLogger("gui"))
            .WithNetwork(network)
            .Build();
        await _guiContainer.StartAsync();
                
        GuiPort = _guiContainer.GetMappedPublicPort(8080);
    }
}