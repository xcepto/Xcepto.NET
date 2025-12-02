using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xcepto.Interfaces;
using Xcepto.States;

namespace Xcepto.Rest
{
    public class XceptoPostRestState : XceptoState
    {
        public XceptoPostRestState(string name, 
            object request,
            Type responseType,
            Uri url,
            Predicate<object> responseValidator
            ) : base(name)
        {
            _url = url;
            _responseType = responseType;
            _request = request;
            _responseValidator = responseValidator;
        }

        private Predicate<object> _responseValidator;
        private object _request;
        private Type _responseType;
        private Uri _url;
        private object _response;

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
            ILoggingProvider loggingProvider = serviceProvider.GetRequiredService<ILoggingProvider>();

            HttpClient client = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(1)
            };

            var requestBody = new StringContent(JsonConvert.SerializeObject(_request), Encoding.UTF8, "application/json");
            loggingProvider.LogDebug($"Send Post request to {_url}");
            var responseMessage = await client.PostAsync(_url, requestBody);
        
            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception($"http request to {_url} faulted");

            var responseString = await responseMessage.Content.ReadAsStringAsync();
            _response = JsonConvert.DeserializeObject(responseString, _responseType);
            if (_response == null) 
                throw new Exception($"http request to {_url} did not return a proper response");
        }
    }
}