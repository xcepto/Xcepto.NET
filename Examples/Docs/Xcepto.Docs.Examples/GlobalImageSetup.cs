namespace Xcepto.Docs.Examples;

[SetUpFixture]
public class GlobalImageSetup
{
    [OneTimeSetUp]
    public async Task BeforeAllTests()
    {
        await DocsApiImageSingleton.GetImageOnce();
    }
}
