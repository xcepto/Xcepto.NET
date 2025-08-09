namespace Samples.Shopping.Api.Contracts.routes;

public class AddSelectedArticleToCartRoute
{
    public string Path => "article/addToCart";
}

public class AddSelectedArticleToCartRequest 
{
    public string ArticleName { get; set; }
}

public class AddSelectedArticleToCartResponse 
{
    public string ArticleName { get; set; }
}