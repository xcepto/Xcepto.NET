using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Images;

namespace Xcepto.Docs.Examples;

public class DocsApiImageSingleton
{
    private static IFutureDockerImage? _image;

    public static async Task<IFutureDockerImage> GetImageOnce()
    {
        if (_image is not null)
            return _image;
        _image = new ImageFromDockerfileBuilder()
            .WithName("xcepto-docs-api:test")
            .WithCleanUp(false)
            .WithDockerfileDirectory(CommonDirectoryPath.GetSolutionDirectory(), Path.Combine("Examples", "Docs"))
            .WithDockerfile("Xcepto.Docs.API/Dockerfile")
            .Build();

        await _image.CreateAsync();
        return _image;
    }
}
