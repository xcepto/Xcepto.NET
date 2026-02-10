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
using Xcepto.States;

namespace Xcepto.SSR
{
    internal class XceptoSsrState : XceptoState
    {
        public XceptoSsrState(string name, 
            Uri url,
            HttpContent? requestBody,
            IEnumerable<HttpResponseAssertion> assertions,
            bool retry,
            HttpClient client,
            HttpMethodVerb methodVerb) : base(name)
        {
            _methodVerb = methodVerb;
            _requestBody = requestBody;
            _retry = retry;
            _url = url;
            _assertions = assertions;
            _client = client;
        }

        private readonly HttpClient _client; 
        private readonly IEnumerable<HttpResponseAssertion> _assertions;
        private readonly Uri _url;
        private readonly bool _retry;
        private readonly HttpContent? _requestBody;

        private HttpResponseMessage? _response;
        private HttpMethodVerb _methodVerb;

        public override async Task<bool> EvaluateConditionsForTransition(IServiceProvider serviceProvider)
        {
            if (_retry)
            {
                if(_response is null)
                    await Execute(serviceProvider);
                if (_response is null)
                    return false;
                if (await CheckAssertions(_response)) 
                    return true;
                _response = null;
                return false;
            }

            await Execute(serviceProvider);
            if (_response is null)
                throw new Exception("Request did not succeed");
            if (await CheckAssertions(_response)) 
                return true;
            return true;
        }

        private async Task<bool> CheckAssertions(HttpResponseMessage response)
        {
            foreach (var assertion in _assertions)
            {
                var selected = await assertion.Selector(response);
                IConstraint resolver = assertion.ResolveConstraint.Resolve();
                var result = resolver.ApplyTo(selected);
                if (!result.IsSuccess)
                {
                    MostRecentFailingResult = new ConditionResult(selected, $"Expected: {resolver.Description}\nBut was: {selected}");
                    return false;
                }
            }
            return true;
        }

        public override Task OnEnter(IServiceProvider serviceProvider) => Task.FromResult(true);
        private async Task Execute(IServiceProvider serviceProvider)
        {
            var loggingProvider = serviceProvider.GetRequiredService<ILoggingProvider>();

            try
            {
                 HttpContent requestBody = _requestBody ?? new StringContent("", Encoding.Default);
                
                switch (_methodVerb)
                {
                    case HttpMethodVerb.Get:
                        _response = await _client.GetAsync(_url);
                        break;
                    case HttpMethodVerb.Post:
                        _response = await _client.PostAsync(_url, requestBody);
                        break;
                    case HttpMethodVerb.Patch:
                        var request = new HttpRequestMessage(new HttpMethod("PATCH"), _url)
                        {
                            Content = requestBody
                        };
                        _response = await _client.SendAsync(request);
                        break;
                    case HttpMethodVerb.Put:
                        _response = await _client.PutAsync(_url, requestBody);
                        break;
                    case HttpMethodVerb.Delete:
                        _response = await _client.DeleteAsync(_url);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception e)
            {
                _response = null;
                loggingProvider.LogDebug(e.Message);
            }
        }
    }
}