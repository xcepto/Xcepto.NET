using System;
using Xcepto.Adapters;
using Xcepto.Builder;
using Xcepto.States;

namespace Xcepto.Interfaces;

public interface IStateMachineBuilder
{
    public void AddFutureStep(Func<XceptoState> futureState, IStateBuilderIdentity stateBuilderIdentity);

    public TXceptoAdapter RegisterAdapter<TXceptoAdapter>(TXceptoAdapter adapter)
        where TXceptoAdapter : XceptoAdapter;
}