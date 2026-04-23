using Serilog;
using TodoApp;
using TodoApp.Services;
using System.Diagnostics;

// Configure Serilog for logging
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
Log.Information("Application started.");

// Parse command-line arguments
var cmdArgs = Environment.GetCommandLineArgs();
Debug.WriteLine($"Args: {string.Join(' ', cmdArgs)}");
if (cmdArgs.Length == 0)
{
    Console.WriteLine("Usage: dotnet run -- <command>");
    Console.WriteLine("  add \"task\"  - add a new task");
    Console.WriteLine("  list          - list all tasks");
    Console.WriteLine("  done <id>     - mark task as done");
    return;
}

// Initialize the DI container and register services
var container = new DIContainer();
Debug.WriteLine("DIContainer initialized.");
container.Register<ITodoService, TodoService>();

// Resolve the ITodoService from the container
var todoService = container.GetService<ITodoService>();
Debug.WriteLine("ITodoService resolved.");

// Handle commands based on the first argument with unified exception handling
try
{
    Debug.WriteLine($"Command: {cmdArgs[1].ToLower()}");
    switch (cmdArgs[1].ToLower())
    {
        case "add":
            if (cmdArgs.Length < 3)
            {
                Console.WriteLine("Please provide a task title.");
                return;
            }
            var title = cmdArgs[2];
            Debug.WriteLine($"Adding task: {title}");
            todoService.Add(title);
            Console.WriteLine($"Added task: {title}");
            break;

        case "list":
            Debug.WriteLine("Listing all tasks.");
            var items = todoService.GetAll();
            if (items.Count == 0)
            {
                Console.WriteLine("No tasks found.");
                return;
            }
            foreach (var item in items)
            {
                Console.WriteLine($"{item.Id}: {item.Title} - {(item.IsCompleted ? "Completed" : "Pending")}");
            }
            break;

        case "done":
            if (cmdArgs.Length < 3)
            {
                Console.WriteLine("Please provide the ID of the task to mark as done.");
                return;
            }
            var id = cmdArgs[2];
            Debug.WriteLine($"Marking task as done: {id}");
            todoService.MarkAsDone(id);
            Console.WriteLine($"Marked task {id} as done.");
            break;

        default:
            Console.WriteLine("Unknown command. Use 'add', 'list', or 'done'.");
            break;
    }
}
catch (Exception ex)
{
    Debug.WriteLine($"Exception: {ex}");
    Console.WriteLine($"Error: {ex.Message}");
    Log.Error(ex, "An error occurred while processing the command.");
}

Log.Information("Application ended.");