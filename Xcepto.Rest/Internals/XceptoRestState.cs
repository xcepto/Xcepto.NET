using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xcepto.Interfaces;
using Xcepto.Internal.Http.Data;
using Xcepto.Internal.Http.States;
using Xcepto.Rest.Data;
using Xcepto.States;

namespace Xcepto.Rest.Internals
{
    internal class XceptoRestState : XceptoHttpState
    {
        public XceptoRestState(string name,
            RequestBody? requestBody,
            Uri url,
            HttpClient client,
            HttpMethodVerb methodVerb,
            bool retry,
            IEnumerable<HttpResponseAssertion> assertions, 
            Func<HttpResponseMessage, Task> responseAction) : base(name, assertions, retry, responseAction)
        {
            if (_methodVerb is HttpMethodVerb.Get or HttpMethodVerb.Delete && _requestBody is not null)
                throw new ArgumentException("GET/DELETE dont support request body");

            _methodVerb = methodVerb;
            _client = client;
            _url = url;
            _requestBody = requestBody;
        }

        private readonly RequestBody? _requestBody;
        private readonly Uri _url;
        private readonly HttpClient _client;
        private readonly HttpMethodVerb _methodVerb;

        protected override async Task<HttpResponseMessage> ExecuteRequest(IServiceProvider serviceProvider)
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
            
            loggingProvider.LogDebug($"Send {_methodVerb} REST request to {_url}");

            HttpResponseMessage response;
            switch (_methodVerb)
            {
                case HttpMethodVerb.Get:
                    response = await _client.GetAsync(_url);
                    break;
                case HttpMethodVerb.Post:
                    response = await _client.PostAsync(_url, requestBody);
                    break;
                case HttpMethodVerb.Patch:
                    var request = new HttpRequestMessage(new HttpMethod("PATCH"), _url)
                    {
                        Content = requestBody
                    };
                    response = await _client.SendAsync(request);
                    break;
                case HttpMethodVerb.Put:
                    response = await _client.PutAsync(_url, requestBody);
                    break;
                case HttpMethodVerb.Delete:
                    response = await _client.DeleteAsync(_url);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return response;
        }

        public override Task OnEnter(IServiceProvider serviceProvider) => Task.CompletedTask;
    }
}