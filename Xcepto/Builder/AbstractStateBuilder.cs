using Xcepto.Interfaces;
using Xcepto.States;

namespace Xcepto.Builder;

public abstract class AbstractStateBuilder<TBuilder>
where TBuilder : AbstractStateBuilder<TBuilder>
{
    private string? _name;
    private bool? _retry;
    protected IStateMachineBuilder StateMachineBuilder;

    protected AbstractStateBuilder(IStateMachineBuilder stateMachineBuilder)
    {
        StateMachineBuilder = stateMachineBuilder;
        stateMachineBuilder.AddFutureStep(Build);
    }

    protected bool Retry => _retry ?? DefaultRetry;
    protected string Name => _name ?? DefaultName;
    protected abstract string DefaultName { get; }
    protected abstract bool DefaultRetry { get; }

    public TBuilder WithCustomName(string name)
    {
        _name = name;
        return (TBuilder)this;
    }
    
    public TBuilder WithRetry(bool retry)
    {
        _retry = retry;
        return (TBuilder)this;
    }

    protected abstract XceptoState Build();
}