using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xcepto.Interfaces;
using Xcepto.States;

namespace Xcepto.SSR
{
    public class XceptoPostSSRState : XceptoState
    {
        public XceptoPostSSRState(string name, 
            HttpContent request,
            Uri url,
            Func<HttpResponseMessage,Task<bool>> responseValidator,
            bool retry
            ) : base(name)
        {
            _retry = retry;
            _url = url;
            _request = request;
            _responseValidator = responseValidator;
        }

        private Func<HttpResponseMessage,Task<bool>> _responseValidator;
        private HttpContent _request;
        private Uri _url;
        private HttpResponseMessage? _response;
        private bool _retry;

        public override async Task<bool> EvaluateConditionsForTransition(IServiceProvider serviceProvider)
        {
            if (_retry)
            {
                if(_response is null)
                    await Execute(serviceProvider);
                if (_response is null)
                    return false;
                if (await _responseValidator(_response)) 
                    return true;
                _response = null;
                return false;
            }

            await Execute(serviceProvider);
            if (_response is null || !await _responseValidator(_response))
                throw new Exception($"Request did not validate successfully: {_response}");
            return true;
        }

        public override Task OnEnter(IServiceProvider serviceProvider) => Task.FromResult(true);
        private async Task Execute(IServiceProvider serviceProvider)
        {
            var loggingProvider = serviceProvider.GetRequiredService<ILoggingProvider>();

            try
            {
                HttpClient client = new HttpClient()
                {
                    Timeout = TimeSpan.FromSeconds(1)
                };

                _response = await client.PostAsync(_url, _request);
            }
            catch (Exception e)
            {
                loggingProvider.LogDebug(e.Message);
            }
        }
    }
}