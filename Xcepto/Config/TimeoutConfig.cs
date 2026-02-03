using System;

namespace Xcepto.Config;

public class TimeoutConfig
{
    public TimeSpan Total { get; }
    public TimeSpan Test { get; }

    public TimeoutConfig(TimeSpan total, TimeSpan test)
    {
        Total = total;
        Test = test;
    }

    public static TimeoutConfig FromSeconds(int i)
    {
        return new TimeoutConfig(TimeSpan.FromSeconds(i), TimeSpan.FromSeconds(i));
    }
}