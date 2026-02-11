using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Xcepto.Data;
using Xcepto.Interfaces;
using Xcepto.Internal.Http.Data;
using Xcepto.Internal.Http.States;
using Xcepto.States;

namespace Xcepto.SSR
{
    internal class XceptoSsrState : XceptoHttpState
    {
        public XceptoSsrState(string name, 
            Uri url,
            HttpContent? requestBody,
            IEnumerable<HttpResponseAssertion> assertions,
            bool retry,
            HttpClient client,
            HttpMethodVerb methodVerb) : base(name, assertions, retry)
        {
            _methodVerb = methodVerb;
            _requestBody = requestBody;
            _url = url;
            _client = client;
        }

        private readonly HttpClient _client; 
        private readonly Uri _url;
        private readonly HttpContent? _requestBody;

        private HttpMethodVerb _methodVerb;

        protected override async Task<HttpResponseMessage> ExecuteRequest(IServiceProvider serviceProvider)
        {
            var loggingProvider = serviceProvider.GetRequiredService<ILoggingProvider>();

            HttpContent requestBody = _requestBody ?? new StringContent("", Encoding.Default);

            loggingProvider.LogDebug($"Send {_methodVerb} SSR request to {_url}");

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