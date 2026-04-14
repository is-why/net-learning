using Microsoft.Extensions.Logging;
using Serilog;

// Create a logger factory and add console logging
using ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

// Create a logger for the Program class
ILogger<Program> logger = loggerFactory.CreateLogger<Program>();

// Log messages of different severity levels
logger.LogInformation("This is an informational message.");
logger.LogWarning("This is a warning message.");
logger.LogError("This is an error message.");

// Create a Serilog logger that writes to a file with daily rolling intervals
var log = new LoggerConfiguration()
    .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Log messages using Serilog
log.Information("This is an informational message from Serilog.");
log.Warning("This is a warning message from Serilog.");
log.Error("This is an error message from Serilog.");