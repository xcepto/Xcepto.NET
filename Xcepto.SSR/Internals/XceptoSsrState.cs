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
            Func<HttpContent>? requestBody,
            IEnumerable<HttpResponseAssertion> assertions,
            bool retry,
            Func<HttpClient> clientProducer,
            HttpMethodVerb methodVerb,
            Func<HttpResponseMessage, Task> responseAction) 
            : base(name, assertions, retry, responseAction)
        {
            _methodVerb = methodVerb;
            _requestBody = requestBody;
            _url = url;
            _clientProducer = clientProducer;
        }

        private readonly Func<HttpClient> _clientProducer; 
        private readonly Uri _url;
        private readonly Func<HttpContent>? _requestBody;

        private HttpMethodVerb _methodVerb;

        protected override async Task<HttpResponseMessage> ExecuteRequest(IServiceProvider serviceProvider)
        {
            var loggingProvider = serviceProvider.GetRequiredService<ILoggingProvider>();

            HttpContent requestBody = _requestBody is not null ? _requestBody() 
                : new StringContent("", Encoding.Default);

            loggingProvider.LogDebug($"Send {_methodVerb} SSR request to {_url}");

            HttpClient client = _clientProducer();
            HttpResponseMessage response;
            switch (_methodVerb)
            {
                case HttpMethodVerb.Get:
                    response = await client.GetAsync(_url);
                    break;
                case HttpMethodVerb.Post:
                    response = await client.PostAsync(_url, requestBody);
                    break;
                case HttpMethodVerb.Patch:
                    var request = new HttpRequestMessage(new HttpMethod("PATCH"), _url)
                    {
                        Content = requestBody
                    };
                    response = await client.SendAsync(request);
                    break;
                case HttpMethodVerb.Put:
                    response = await client.PutAsync(_url, requestBody);
                    break;
                case HttpMethodVerb.Delete:
                    response = await client.DeleteAsync(_url);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return response;
        }
        
        public override Task OnEnter(IServiceProvider serviceProvider) => Task.CompletedTask;
    }
}