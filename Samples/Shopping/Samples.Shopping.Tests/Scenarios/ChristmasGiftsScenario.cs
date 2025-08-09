using Microsoft.Extensions.DependencyInjection;
using Samples.Shopping.Tests.providers;
using Xcepto;
using Xcepto.Interfaces;

namespace Samples.Shopping.Tests.scenarios;

public class ChristmasGiftsScenario: Scenario
{
    public override IServiceCollection Setup()
    {
        LoggingProvider loggingProvider = new LoggingProvider();
        
        ServiceCollection services = new ServiceCollection();
        services.AddSingleton<ILoggingProvider, LoggingProvider>(x => loggingProvider);
        return services;
    }

    // public void DoGoToHomePage()
    // {
    //     stateMachine.SetOnEnterAction(action => action
    //         .ExecuteRestAction<GoToHomepageRequest, GoToHomepageResponse>(new GoToHomepageRequest()
    //             {
    //                 Trace = 7
    //             }, 
    //             response => response.Trace == 7));
    //     
    //     stateMachine.AddTransition(transition => transition
    //         .DependsOn<ClientVisitedHomepageEvent>(x => true)
    //         // .DependsOnClientVisitedHomepageEvent(x => true)
    //     );
    // }
    //
    // public void DoSearchForArticle(string articleName)
    // {
    //     stateMachine.SetOnEnterAction(action => action
    //         .ExecuteRestAction<SearchForArticleRequest, SearchForArticleResponse>(new SearchForArticleRequest()
    //         {
    //             ArticleName = articleName
    //         }, response => response.Message == "Articles are being searched"));
    //     stateMachine.AddTransition(transition => transition
    //         .DependsOn<SearchSearchedForArticle>(x=> x.ArticleName == articleName)
    //         // .DependsOnSearchSearchedForArticle(x => x.ArticleName == articleName)
    //     );
    // }
    //
    // public void DoSelectNthArticleInList(int articleIndex)
    // {
    //     stateMachine.AddTransition(transition =>
    //         {
    //             transition.MakeUnconditionalTransition();
    //         }
    //     );
    // }
    //
    // public void DoAddSelectedArticleToCart()
    // {
    //     var christmasTree = "Christmas tree";
    //     stateMachine.SetOnEnterAction(action => action
    //         .ExecuteRestAction<AddSelectedArticleToCartRequest, AddSelectedArticleToCartResponse>(new AddSelectedArticleToCartRequest()
    //         {
    //             ArticleName = christmasTree
    //         }, response => response.ArticleName == christmasTree));
    //     
    //     stateMachine.AddTransition(transition => transition
    //         .DependsOn<ClientAddedSelectedArticleToCart>(x => true)
    //         // .DependsOnClientAddedSelectedArticleToCart(x=> true)
    //     );
    // }
    //
    // public void DoGoBackToSearchResults()
    // {
    //     stateMachine.AddTransition(transition =>
    //         {
    //             transition.MakeUnconditionalTransition();
    //         }
    //     );
    // }
    //
    // public void ExpectArticleCountInShoppingCart(int count)
    // {
    //     stateMachine.AddTransition(transition => transition
    //         .DependsOnCount<CartAddedArticleToCart>(x=> true, count)
    //         // .DependsOnCartAddedArticleToCartCount(x => true, count)
    //     );
    // }
}