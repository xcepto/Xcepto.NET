using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xcepto.Interfaces;
using Xcepto.Rest.Data;
using Xcepto.States;

namespace Xcepto.Rest.Internals
{
    internal class XceptoPostRestState : XceptoState
    {
        public XceptoPostRestState(string name, 
            RequestBody? requestBody,
            ResponseValidation? responseValidation,
            Uri url,
            HttpClient client,
            RestHttpMethod method
            ) : base(name)
        {
            if (_method is RestHttpMethod.Get or RestHttpMethod.Delete && _requestBody is not null)
                throw new ArgumentException("GET/DELETE dont support request body");

            _method = method;
            _client = client;
            _url = url;
            _responseValidation = responseValidation;
            _requestBody = requestBody;
        }

        private readonly RequestBody? _requestBody;
        private readonly ResponseValidation? _responseValidation;
        private readonly Uri _url;
        private readonly HttpClient _client;
        private readonly RestHttpMethod _method;
        private object _response;

        public override Task<bool> EvaluateConditionsForTransition(IServiceProvider serviceProvider)
        {
            if (_responseValidation is not null)
            {
                if (!_responseValidation.ValidationMethod(_response))
                    throw new Exception("response was not validated successfully");   
            }
            return Task.FromResult(true);
        }

        public override async Task OnEnter(IServiceProvider serviceProvider)
        {
            await Execute(serviceProvider);
        }
        
        private async Task Execute(IServiceProvider serviceProvider)
        {
            ILoggingProvider loggingProvider = serviceProvider.GetRequiredService<ILoggingProvider>();
            
            StringContent requestBody;
            if (_requestBody is null)
            {
                requestBody = new StringContent("", Encoding.Default);
            }
            else
            {
                requestBody = new StringContent(_requestBody.SerializationMethod(_requestBody.RequestObject),
                    Encoding.UTF8, "application/json");
            }
            
            loggingProvider.LogDebug($"Send Post request to {_url}");

            HttpResponseMessage responseMessage;
            switch (_method)
            {
                case RestHttpMethod.Get:
                    responseMessage = await _client.GetAsync(_url);
                    break;
                case RestHttpMethod.Post:
                    responseMessage = await _client.PostAsync(_url, requestBody);
                    break;
                case RestHttpMethod.Patch:
                    var request = new HttpRequestMessage(new HttpMethod("PATCH"), _url)
                    {
                        Content = requestBody
                    };
                    responseMessage = await _client.SendAsync(request);
                    break;
                case RestHttpMethod.Put:
                    responseMessage = await _client.PutAsync(_url, requestBody);
                    break;
                case RestHttpMethod.Delete:
                    responseMessage = await _client.DeleteAsync(_url);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        
            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception($"http request to {_url} faulted");

            if (_responseValidation is not null)
            {
                var responseString = await responseMessage.Content.ReadAsStringAsync();
                _response = _responseValidation.DeserializationMethod(responseString);
                if (_response == null) 
                    throw new Exception($"http request to {_url} did not return a proper response");
            }
        }
    }
}