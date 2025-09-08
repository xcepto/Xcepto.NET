﻿using ExceptionPropagation.Tests.Exceptions;
using Xcepto;

namespace ExceptionPropagation.Tests.States;

public class PropagatedState: XceptoState
{
    public PropagatedState(string name) : base(name)
    {
    }
    public override Task<bool> EvaluateConditionsForTransition(IServiceProvider serviceProvider) =>
        Task.FromResult(true);

    public override Task OnEnter(IServiceProvider serviceProvider)
    {
        PropagateExceptions(Task.Run(() =>
        {
            throw new PropagatedException();
        }));
        return Task.CompletedTask;
    }
}