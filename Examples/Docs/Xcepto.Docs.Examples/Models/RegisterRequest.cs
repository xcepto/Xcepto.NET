using System.Collections.Generic;
using System.Net.Http;

namespace Xcepto.Docs.Examples;

public record RegisterRequest(string Username, string Password)
{
    public FormUrlEncodedContent ToForm() =>
        new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["Username"] = Username,
            ["Password"] = Password,
        });
}
