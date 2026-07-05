using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Images;

namespace Samples.Rest.API.Tests.Images;

public class ImageSingleton
{
    private static IFutureDockerImage? _apiImage;

    public static async Task<IFutureDockerImage> CreateApiImageOnce()
    {
        if (_apiImage is not null)
            return _apiImage;
        _apiImage = new ImageFromDockerfileBuilder()
            .WithName("examples-restauth-api:test")
            .WithCleanUp(false)
            .WithDockerfileDirectory(CommonDirectoryPath.GetSolutionDirectory(), Path.Combine("Examples", "RestAuth"))
            .WithDockerfile("Samples.Rest.API/Dockerfile")
            .Build();

        await _apiImage.CreateAsync();
        return _apiImage;
    }
}