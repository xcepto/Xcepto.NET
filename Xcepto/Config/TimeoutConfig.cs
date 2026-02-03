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

    public static TimeoutConfig FromSeconds(float total, float test)
    {
        return new TimeoutConfig(TimeSpan.FromSeconds(total), TimeSpan.FromSeconds(test));
    }
    
    public static TimeoutConfig FromSeconds(float total)
    {
        if(total >= 1)
            return FromSeconds(total, total - 0.5f);
        return FromSeconds(total, total);
    }
}