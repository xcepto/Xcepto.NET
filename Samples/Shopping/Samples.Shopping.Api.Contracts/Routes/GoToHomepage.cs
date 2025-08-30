namespace Samples.Shopping.Api.Contracts.Routes
{
    public class GoToHomepageRoute
    {
        public string Path => "go-to-homepage";
    }
    
    public class GoToHomepageResponse
    {
        public int Trace { get; set; }
    }
    
    public class GoToHomepageRequest
    {
        public int Trace { get; set; }
    }
}