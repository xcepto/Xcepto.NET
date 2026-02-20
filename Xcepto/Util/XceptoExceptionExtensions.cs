using System;
using System.Reflection;
using Xcepto.Exceptions;

namespace Xcepto.Util;

public static class XceptoExceptionExtensions
{
    public static XceptoStageException Promote(this XceptoStageException outer, Exception inner)
    {
        CopyStackTrace(inner, outer);
        SetInnerException(outer, inner);
        ClearStackTrace(inner);
        
        return outer;
    }
    
    private static void SetInnerException(Exception outer, Exception inner)
    {
        var field = typeof(Exception)
            .GetField("_innerException",
                BindingFlags.Instance | BindingFlags.NonPublic);

        field!.SetValue(outer, inner);
    }
    
    private static void ClearStackTrace(Exception ex)
    {
        var stackTraceField =
            typeof(Exception).GetField("_stackTrace",
                BindingFlags.Instance | BindingFlags.NonPublic);

        var stackTraceStringField =
            typeof(Exception).GetField("_stackTraceString",
                BindingFlags.Instance | BindingFlags.NonPublic);

        stackTraceField?.SetValue(ex, null);
        stackTraceStringField?.SetValue(ex, null);
    }
    
    private static void CopyStackTrace(Exception source, Exception target)
    {
        var field = typeof(Exception)
            .GetField("_remoteStackTraceString",
                BindingFlags.Instance | BindingFlags.NonPublic);

        var stackTrace = source.StackTrace + Environment.NewLine;

        field!.SetValue(target, stackTrace);
    }
}