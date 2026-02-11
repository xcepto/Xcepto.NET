using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework.Constraints;
using Xcepto.Data;
using Xcepto.Internal.Http.Data;
using Xcepto.States;

namespace Xcepto.Internal.Http.States
{
    public abstract class XceptoHttpState: XceptoState
    {
        private IEnumerable<HttpResponseAssertion> _assertions;
        private bool _retry;

        protected XceptoHttpState(string name, IEnumerable<HttpResponseAssertion> assertions, bool retry) : base(name)
        {
            _retry = retry;
            _assertions = assertions;
        }
        
        protected async Task<bool> CheckAssertions(HttpResponseMessage response)
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
        
        public override async Task<bool> EvaluateConditionsForTransition(IServiceProvider serviceProvider)
        {
            if (_retry)
            {
                var response = await ExecuteRequest(serviceProvider);
                if (await CheckAssertions(response)) 
                    return true;
                return false;
            }
            else
            {
                var response = await ExecuteRequest(serviceProvider);
                if (response is null)
                    throw new Exception("Request did not succeed");
                if (await CheckAssertions(response)) 
                    return true;
                return false;
            }
        }

        protected abstract Task<HttpResponseMessage> ExecuteRequest(IServiceProvider serviceProvider);
    }
}