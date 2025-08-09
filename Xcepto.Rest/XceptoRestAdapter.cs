using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Xcepto.Rest
{
    public class XceptoRestAdapter: XceptoAdapter
    {
        public XceptoState PostRequest<TRequest, TResponse>(Uri url, TRequest request, Predicate<TResponse> responseValidator)
        {
            Predicate<object> validator = response =>
            {
                if (response is TResponse castedResponse)
                    return responseValidator(castedResponse);
                return false;
            };
            return new XceptoPostRestState($"Post{typeof(TRequest).Name}State", 
                request,
                typeof(TResponse),
                url,
                validator 
                );
        }

        protected override Task Initialize(IServiceProvider serviceProvider) => Task.CompletedTask;
        protected override Task AddServices(IServiceCollection serviceCollection) => Task.CompletedTask;
    }
}