using Xcepto.Adapters;
using Xcepto.Builder;
using Xcepto.Interfaces;

namespace Xcepto.Docs.Examples;

public enum OrderStatus { Fulfilled, Pending }

public sealed class OrderAdapter : XceptoAdapter
{
    public OrderFlowBuilder Order(string orderId)
        => new OrderFlowBuilder(Builder).WithOrderId(orderId);
}

public sealed class OrderAdapterBuilder : AbstractAdapterBuilder<OrderAdapterBuilder, OrderAdapter>
{
    public OrderAdapterBuilder(IStateMachineBuilder builder) : base(builder) { }

    public override OrderAdapter Build()
    {
        var adapter = new OrderAdapter();
        StateMachineBuilder.RegisterAdapter(adapter);
        return adapter;
    }
}

public sealed class OrderFlowBuilder
{
    private readonly IStateMachineBuilder _builder;
    private string _orderId = "";
    private int _amount;

    public OrderFlowBuilder(IStateMachineBuilder builder) { _builder = builder; }
    public OrderFlowBuilder WithOrderId(string id) { _orderId = id; return this; }
    public OrderFlowBuilder WithAmount(int amount) { _amount = amount; return this; }
    public void ShouldReachStatus(OrderStatus status) { /* would add state */ }
}
