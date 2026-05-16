using Api.Extensions;
using Api.Middleware;
using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddVaultSecretAuth(builder.Configuration);
builder.Services.AddVaultSecretRateLimiting();
builder.Services.AddVaultSecretSwagger();

var app = builder.Build();

await app.InitializeDatabaseAsync();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapEndpoints();

app.MapFallbackToFile("index.html");

app.Run();
