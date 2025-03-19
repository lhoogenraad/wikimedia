using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/api/healthcheck", () => Results.Ok("Biggity boogity"));

app.Run();

