using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using Samples.RestAuth.API.Responses;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

var tokenHashHexString = Environment.GetEnvironmentVariable("TOKENHASH");

app.MapGet("/api/ping", () =>
{
    Results.Ok();
});


app.MapPost("/api/authenticated", (HttpContext httpContext) =>
{
    var auth = httpContext.Request.Headers.Authorization.ToString();

    if (string.IsNullOrWhiteSpace(auth) || !auth.StartsWith("Bearer "))
        return Results.Unauthorized();
    var token = auth.Substring("Bearer ".Length).Trim();

    var expectedHash = Convert.FromHexString(tokenHashHexString);
    var actualHash = SHA256.Create().ComputeHash(UrlDecode(token));
    
    Console.WriteLine($"Expected TOKENHASH={tokenHashHexString}");
    Console.WriteLine($"Actual TOKENHASH  ={Convert.ToHexString(SHA256.Create().ComputeHash(UrlDecode(token)))}");
    
    if (!CryptographicOperations.FixedTimeEquals(expectedHash, actualHash))
        return Results.Unauthorized();
    return Results.Json(new AuthenticatedTestResponse());
});

static byte[] UrlDecode(string s)
{
    s = s.Replace("-", "+").Replace("_", "/");
    switch (s.Length % 4)
    {
        case 2: s += "=="; break;
        case 3: s += "="; break;
    }
    return Convert.FromBase64String(s);
}

app.Run();

