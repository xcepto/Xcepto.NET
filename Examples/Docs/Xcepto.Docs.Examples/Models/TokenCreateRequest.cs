using System.Collections.Generic;
using System.Net.Http;

namespace Xcepto.Docs.Examples;

public record TokenCreateRequest(string Name)
{
    public FormUrlEncodedContent ToForm() =>
        new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["Name"] = Name,
        });
}
