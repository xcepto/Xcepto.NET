using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xcepto.Exceptions;

namespace Xcepto.Rest
{
    public class XceptoRestAdapter: XceptoAdapter
    {
        private TransitionBuilder? _builder;
        
        public void PostRequest<TRequest, TResponse>(Uri url, TRequest request, Predicate<TResponse> responseValidator)
        {
            if (_builder is null)
                throw new AdapterException("Builder was not assigned yet");
            Predicate<object> validator = response =>
            {
                if (response is TResponse castedResponse)
                    return responseValidator(castedResponse);
                return false;
            };
            _builder.AddStep(new XceptoPostRestState($"Post{typeof(TRequest).Name}State", 
                request,
                typeof(TResponse),
                url,
                validator 
            ));
        }

        public override void AssignBuilder(TransitionBuilder builder)
        {
            _builder = builder;
        }

        protected override Task Initialize(IServiceProvider serviceProvider) => Task.CompletedTask;
        protected override Task AddServices(IServiceCollection serviceCollection) => Task.CompletedTask;
    }
}