using Samples.SSR.GUI.Tests.Images;

namespace Samples.SSR.GUI.Tests;

[SetUpFixture]
public class GlobalImageSetup
{
    [OneTimeSetUp]
    public async Task BeforeAllTests()
    {
        await ImageSingleton.CreateApiImageOnce();
    }
}