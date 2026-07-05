using Samples.Rest.API.Tests.Images;

namespace Samples.Rest.API.Tests;

[SetUpFixture]
public class GlobalImageSetup
{
    [OneTimeSetUp]
    public async Task BeforeAllTests()
    {
        await ImageSingleton.CreateApiImageOnce();
    }
}