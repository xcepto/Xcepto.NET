using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var tokens = new ConcurrentDictionary<string, string>(); // accessToken → username

app.MapGet("/api/ping", () => Results.Ok());

app.MapPost("/shipment/accept", async (HttpContext ctx) =>
{
    var req = await ctx.Request.ReadFromJsonAsync<AmountRequest>();
    return Results.Json(new { amount = req!.Amount });
});

app.MapGet("/inventory/stock", () =>
    Results.Json(new { replenished = true }));

app.MapPost("/auth/login", async (HttpContext ctx) =>
{
    if (ctx.Request.ContentType?.Contains("json") == true)
    {
        var req = await ctx.Request.ReadFromJsonAsync<LoginRequest>();
        var token = Guid.NewGuid().ToString("N");
        tokens[token] = req!.Username;
        return Results.Json(new { accessToken = token });
    }
    var form = await ctx.Request.ReadFormAsync();
    var username = form["Username"].ToString();
    ctx.Response.Cookies.Append("xcepto-session", username);
    return Results.Ok();
});

app.MapGet("/api/profile", (HttpContext ctx) =>
{
    var auth = ctx.Request.Headers.Authorization.ToString();
    if (!auth.StartsWith("Bearer ")) return Results.Unauthorized();
    var token = auth["Bearer ".Length..].Trim();
    return tokens.TryGetValue(token, out var username)
        ? Results.Json(new { username })
        : Results.Unauthorized();
});

app.MapPost("/auth/register", () => Results.Ok());

app.MapGet("/dashboard", (HttpContext ctx) =>
{
    var username = ctx.Request.Cookies["xcepto-session"] ?? "guest";
    return Results.Content($"<html><body><p>{username}</p></body></html>", "text/html");
});

app.MapPost("/token/create", () =>
{
    var token = Guid.NewGuid().ToString("N");
    return Results.Content($"<html><body><span>{token}</span></body></html>", "text/html");
});

app.MapPost("/api/env/create", (HttpContext ctx) =>
{
    var auth = ctx.Request.Headers.Authorization.ToString();
    return string.IsNullOrEmpty(auth) ? Results.Unauthorized() : Results.Ok();
});

app.Run();

record AmountRequest(int Amount);
record LoginRequest(string Username, string Password);
