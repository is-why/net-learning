using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<EnvSettings>(builder.Configuration.GetSection("EnvSettings"));

var app = builder.Build();

app.MapGet("/env", (IConfiguration configuration, IOptionsMonitor<EnvSettings> options) =>
{
    var envNameFromConfig = configuration["EnvSettings:EnvName"] ?? "Default";
    var envNameFromOptions = options.CurrentValue.EnvName;

    return new {
        EnvNameFromConfig = envNameFromConfig,
        EnvNameFromOptions = envNameFromOptions
    };
});

app.Run();

public class EnvSettings
{
    public string EnvName { get; set; } = string.Empty;
}