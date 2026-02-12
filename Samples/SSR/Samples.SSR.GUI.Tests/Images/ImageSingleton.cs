using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Images;

namespace Samples.SSR.GUI.Tests.Images;

public class ImageSingleton
{
    private static IFutureDockerImage? _apiImage;

    public static async Task<IFutureDockerImage> CreateApiImageOnce()
    {
        if (_apiImage is not null)
            return _apiImage;
        _apiImage = new ImageFromDockerfileBuilder()
            .WithName("samples-ssr-gui:test")
            .WithDockerfileDirectory(CommonDirectoryPath.GetSolutionDirectory(), Path.Combine("Samples", "SSR"))
            .WithDockerfile("Samples.SSR.GUI/Dockerfile")
            .Build();

        await _apiImage.CreateAsync();
        return _apiImage;
    }
}