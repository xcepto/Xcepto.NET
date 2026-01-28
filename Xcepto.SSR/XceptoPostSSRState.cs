using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xcepto.States;

namespace Xcepto.SSR
{
    public class XceptoPostSSRState : XceptoState
    {
        public XceptoPostSSRState(string name, 
            HttpContent request,
            Uri url,
            Predicate<HttpContent> responseValidator
            ) : base(name)
        {
            _url = url;
            _request = request;
            _responseValidator = responseValidator;
        }

        private Predicate<HttpContent> _responseValidator;
        private HttpContent _request;
        private Uri _url;
        private HttpContent _response;

        public override Task<bool> EvaluateConditionsForTransition(IServiceProvider serviceProvider)
        {
            if (!_responseValidator(_response))
                throw new Exception("response was not validated successfully");
            return Task.FromResult(true);
        }

        public override async Task OnEnter(IServiceProvider serviceProvider)
        {
            await Execute(serviceProvider);
        }
        
        private async Task Execute(IServiceProvider serviceProvider)
        {
            HttpClient client = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(1)
            };

            var responseMessage = await client.PostAsync(_url, _request);
        
            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception($"http request to {_url} faulted");

            _response = responseMessage.Content;
            if (_response == null) 
                throw new Exception($"http request to {_url} did not return a proper response");
        }
    }
}