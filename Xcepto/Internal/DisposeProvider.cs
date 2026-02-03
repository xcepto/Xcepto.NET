using System;
using System.Collections.Generic;

namespace Xcepto.Internal;

internal class DisposeProvider()
{
    private List<IDisposable> _disposables = new();
    internal void DisposeAll()
    {
        foreach (var disposable in _disposables)
        {
            disposable.Dispose();
        }
    }

    internal void Add(IDisposable disposable)
    {
        _disposables.Add(disposable);
    }
}