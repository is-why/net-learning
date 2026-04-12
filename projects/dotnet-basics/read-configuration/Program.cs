using Microsoft.Extensions.Configuration;

// get environment variable override
Console.WriteLine("=== Environment Variable Override Test ===");
var envOverride = Environment.GetEnvironmentVariable("Database");
if (!string.IsNullOrEmpty(envOverride))
    Console.WriteLine($"Environment variable override: {envOverride}");
else
    Console.WriteLine("Environment variable Database not set");

// build configuration with JSON and environment variables
Console.WriteLine("\n=== Reading Configuration ===");
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

// simple key-value access
Console.WriteLine($"Database: {config["Database"]}");
Console.WriteLine($"Timeout: {config["Timeout"]}");

// nested configuration access
var loggingSection = config.GetSection("Ports");
foreach (var kvp in loggingSection.GetChildren())
{
    Console.WriteLine($"Port:{kvp.Key} = {kvp.Value}");
}