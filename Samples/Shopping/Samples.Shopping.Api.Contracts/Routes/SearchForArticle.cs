namespace Samples.Shopping.Api.Contracts.routes;

public class SearchForArticleRoute
{
    public string Path => "search/article";
}


public class SearchForArticleRequest 
{
    public string ArticleName { get; set; }
}

public class SearchForArticleResponse 
{
    public string Message { get; set; }
}