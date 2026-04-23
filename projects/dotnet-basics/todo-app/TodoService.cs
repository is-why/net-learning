using System.Text.Json;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using Serilog;

namespace TodoApp.Services
{
    public interface ITodoService
    {
        List<TodoItem> GetAll();
        void Add(string title);
        void MarkAsDone(string id);
    }

    public class TodoService : ITodoService
    {
        private readonly IConfiguration _configuration;
        private readonly string _storageFilePath = "todoitems.json";
        private readonly int _maxItems;
        private readonly List<TodoItem> _todoItems;
        private static readonly ILogger _logger = Log.ForContext<TodoService>();

        public TodoService()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            _maxItems = int.TryParse(_configuration["maxItems"], out var max) ? max : 100;
            var configPath = _configuration["dataFilePath"];
            if (!string.IsNullOrWhiteSpace(configPath))
            {
                _storageFilePath = configPath;
            }

            if (File.Exists(_storageFilePath))
            {
                try
                {
                    var json = File.ReadAllText(_storageFilePath);
                    _todoItems = JsonSerializer.Deserialize<List<TodoItem>>(json, new JsonSerializerOptions { WriteIndented = true }) ?? new List<TodoItem>();
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Failed to load todo items from {FilePath}", _storageFilePath);
                    _todoItems = new List<TodoItem>();
                }
            }
            else
            {
                _todoItems = new List<TodoItem>();
            }
        }

        public void Add(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                _logger.Warning("Attempted to add a todo with empty title.");
                throw new ArgumentException("Title cannot be empty.", nameof(title));
            }
            if (_todoItems.Count >= _maxItems)
            {
                _logger.Warning("Add failed: max items reached ({MaxItems})", _maxItems);
                throw new InvalidOperationException($"Cannot add more than {_maxItems} todo items.");
            }

            var newItem = new TodoItem { Title = title };
            _todoItems.Add(newItem);
            SaveTodoItems();
            _logger.Information("Task added: {Title}", title);
        }

        public List<TodoItem> GetAll()
        {
            return _todoItems.OrderByDescending(item => item.Id).ToList();
        }

        public void MarkAsDone(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.Warning("MarkAsDone called with empty id.");
                throw new ArgumentException("Id cannot be empty.", nameof(id));
            }
            var item = _todoItems.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                if (!item.IsCompleted)
                {
                    item.IsCompleted = true;
                    SaveTodoItems();
                    _logger.Information("Task marked as done: {Id}", id);
                }
                else
                {
                    _logger.Information("Task already completed: {Id}", id);
                }
            }
            else
            {
                _logger.Warning("Task not found: {Id}", id);
                throw new InvalidOperationException($"Task with id {id} not found.");
            }
        }

        private void SaveTodoItems()
        {
            try
            {
                var json = JsonSerializer.Serialize(_todoItems, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_storageFilePath, json);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to save todo items to {FilePath}", _storageFilePath);
                throw;
            }
        }
    }

    public class TodoItem
    {
        public string Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }

        public TodoItem()
        {
            Id = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
        }
    }
}