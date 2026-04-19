using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OptionsPattern;

// create configuration from appsettings.json
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// register DatabaseSettings with IOptions
var services = new ServiceCollection()
    .Configure<DatabaseSettings>(configuration.GetSection(nameof(DatabaseSettings)));
var serviceProvider = services.BuildServiceProvider();

// read settings using IOptions
var options = serviceProvider.GetRequiredService<IOptions<DatabaseSettings>>();
Console.WriteLine($"ConnectionString: {options.Value.ConnectionString}");
Console.WriteLine($"Timeout: {options.Value.Timeout}");