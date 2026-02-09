using Xcepto.Interfaces;
using Xcepto.States;

namespace Xcepto.Builder;

public abstract class AbstractStateBuilder<TBuilder>
where TBuilder : AbstractStateBuilder<TBuilder>
{
    private string? _name;

    protected AbstractStateBuilder(IStateMachineBuilder stateMachineBuilder)
    {
        stateMachineBuilder.AddFutureStep(Build);
    }

    protected string Name => _name ?? DefaultName;
    protected abstract string DefaultName { get; }

    public TBuilder WithCustomName(string name)
    {
        _name = name;
        return (TBuilder)this;
    }

    protected abstract XceptoState Build();
}