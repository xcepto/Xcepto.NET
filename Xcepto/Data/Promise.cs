using Xcepto.Exceptions;
using Xcepto.Interfaces;

namespace Xcepto.Data;

public class Promise<TData>
{
    private bool _settled = false;
    private TData _data = default!;

    public TData Resolve()
    {
        if (!_settled)
            throw new PromiseException("Promise not fulfilled yet");
        return _data;
    }

    public void Settle(TData data)
    {
        if (_settled)
            throw new PromiseException("Promise already fulfilled");
        _data = data;
        _settled = true;
    }
}