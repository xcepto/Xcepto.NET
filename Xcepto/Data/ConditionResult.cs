using System;

namespace Xcepto.Data;

public class ConditionResult
{
    public object Actual { get; }
    public string FailureDescription { get; }

    public ConditionResult(object actual, string failureDescription)
    {
        Actual = actual;
        FailureDescription = failureDescription;
    }
}