using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xcepto.Exceptions;

namespace Xcepto.Rest
{
    public class XceptoRestAdapter: XceptoAdapter
    {
        
        public void PostRequest<TRequest, TResponse>(Uri url, TRequest request, Predicate<TResponse> responseValidator)
        {
            Predicate<object> validator = response =>
            {
                if (response is TResponse castedResponse)
                    return responseValidator(castedResponse);
                return false;
            };
            AddStep(new XceptoPostRestState($"Post{typeof(TRequest).Name}State", 
                request,
                typeof(TResponse),
                url,
                validator 
            ));
        }
    }
}