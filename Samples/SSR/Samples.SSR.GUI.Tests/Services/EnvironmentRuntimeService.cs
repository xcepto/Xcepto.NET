using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Testcontainers.PostgreSql;

namespace Samples.SSR.GUI.Tests.Services;

public class EnvironmentRuntimeService
{
    private PostgreSqlContainer? _postgres;
    private IContainer? _gui;
    public int GuiPort { get; private set; }
    public int PostgresPort { get; private set; }
    public async Task Start()
    {
        var guiImage = new ImageFromDockerfileBuilder()
            .WithName("samples-ssr-gui:test")
            .WithDockerfileDirectory(CommonDirectoryPath.GetSolutionDirectory(), string.Empty)
            .WithDockerfile("Samples/SSR/Samples.SSR.GUI/Dockerfile")
            .Build();
        await guiImage.CreateAsync();
        
        _postgres = new PostgreSqlBuilder("postgres:16")
            .WithDatabase("test")
            .WithUsername("test")
            .WithPassword("test")
            .WithPortBinding(5432, true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilExternalTcpPortIsAvailable(5432))
            .Build();
        
        await _postgres.StartAsync();
        
        PostgresPort = _postgres.GetMappedPublicPort(5432);

        _gui = new ContainerBuilder(guiImage)
            .WithPortBinding(8080, true)
            .WithEnvironment("Postgres__Host", "host.docker.internal")
            .WithEnvironment("Postgres__Port", PostgresPort.ToString())
            .WithWaitStrategy(Wait.ForUnixContainer().UntilExternalTcpPortIsAvailable(8080))
            .Build();
        await _gui.StartAsync();

        GuiPort = _gui.GetMappedPublicPort(8080);
    }
}