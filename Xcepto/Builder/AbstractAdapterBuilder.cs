using Xcepto.Adapters;
using Xcepto.Interfaces;

namespace Xcepto.Builder;

public abstract class AbstractAdapterBuilder<TAdapterBuilder, TAdapter>
where TAdapterBuilder: AbstractAdapterBuilder<TAdapterBuilder, TAdapter>
where TAdapter: XceptoAdapter
{
    protected IStateMachineBuilder StateMachineBuilder;

    public AbstractAdapterBuilder(IStateMachineBuilder stateMachineBuilder)
    {
        StateMachineBuilder = stateMachineBuilder;
    }

    public abstract TAdapter Build();
}