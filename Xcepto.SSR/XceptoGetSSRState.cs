using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xcepto.Interfaces;
using Xcepto.States;

namespace Xcepto.SSR
{
    public class XceptoGetSSRState : XceptoState
    {
        public XceptoGetSSRState(string name, 
            Uri url,
            Func<HttpContent,Task<bool>> responseValidator,
            bool retry
            ) : base(name)
        {
            _retry = retry;
            _url = url;
            _responseValidator = responseValidator;
        }

        private Func<HttpContent,Task<bool>> _responseValidator;
        private Uri _url;
        private HttpContent? _response;
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
            if (_response is null || await _responseValidator(_response))
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

                var responseMessage = await client.GetAsync(_url);
        
                if (!responseMessage.IsSuccessStatusCode)
                    throw new Exception($"http request to {_url} faulted");

                _response = responseMessage.Content;
                if (_response == null) 
                    throw new Exception($"http request to {_url} did not return a proper response");
            }
            catch (Exception e)
            {
                loggingProvider.LogDebug(e.Message);
            }
        }
    }
}