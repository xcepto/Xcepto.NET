using System.Threading.Tasks;
using Xcepto.Interfaces;
using Xcepto.Rest.Internals;
using Xcepto.States;

namespace Xcepto.Rest.Builders;

public sealed class RestStateBuilderIdentity: RestStateBuilderIdentity<RestStateBuilderIdentity>
{
    internal RestStateBuilderIdentity(IStateMachineBuilder stateMachineBuilder) : base(stateMachineBuilder)
    {
    }
    
    protected override XceptoState Build()
    {
        return new XceptoRestState(Name, RequestBody, Url, ClientProducer, MethodVerb, Retry, ResponseAssertions, _ => Task.CompletedTask);
    }
}