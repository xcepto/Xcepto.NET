using System.Linq;
using System.Net.Http;

namespace Xcepto.SSR.Extensions;

public static class SsrExtensions
{
    public static FormUrlEncodedContent ToForm(this object recordObj)
    {
        var dict = recordObj.GetType()
            .GetProperties()
            .ToDictionary(
                p => p.Name,
                p => p.GetValue(recordObj)?.ToString() ?? ""
            );

        return new FormUrlEncodedContent(dict);
    }
}