using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xcepto.Adapters;

namespace Xcepto.Rest
{
    public class XceptoRestAdapter: XceptoAdapter
    {
        private HttpClient _client;

        public XceptoRestAdapter(HttpClient client)
        {
            _client = client;
        }
        public XceptoRestAdapter()
        {
            _client = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(1)
            };
        }
        public XceptoRestAdapter(string bearerToken)
        {
            _client = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(1),
                DefaultRequestHeaders =
                {
                    Authorization = new AuthenticationHeaderValue("Bearer", bearerToken)
                }
            };
        }
        
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
                validator,
                _client
            ));
        }
    }
}