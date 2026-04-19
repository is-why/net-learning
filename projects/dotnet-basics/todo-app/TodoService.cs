using System.Text.Json;
using Microsoft.Extensions.Configuration;

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
        private readonly string _storageFilePath;
        private readonly int _maxItems;
        private readonly List<TodoItem> _todoItems;

        public TodoService()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            _maxItems = int.Parse(_configuration["maxItems"] ?? "100");
            _storageFilePath = _configuration["dataFilePath"] ?? "todoitems.json";

            // Load existing todo items from storage
            if (File.Exists(_storageFilePath))
            {
                var json = File.ReadAllText(_storageFilePath);
                _todoItems = JsonSerializer.Deserialize<List<TodoItem>>(json, options: new JsonSerializerOptions { WriteIndented = true }) ?? new List<TodoItem>();
            }
            else
            {
                _todoItems = new List<TodoItem>();
            }
        }

        public void Add(string title)
        {
            if (_todoItems.Count >= _maxItems)
            {
                throw new InvalidOperationException($"Cannot add more than {_maxItems} todo items.");
            }

            var newItem = new TodoItem { Title = title };
            _todoItems.Add(newItem);
            SaveTodoItems();
        }

        public List<TodoItem> GetAll()
        {
            return _todoItems.OrderByDescending(item => item.Id).ToList();
        }

        public void MarkAsDone(string id)
        {
            var item = _todoItems.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                item.IsCompleted = true;
                SaveTodoItems();
            }
        }

        private void SaveTodoItems()
        {
            var json = JsonSerializer.Serialize(_todoItems, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_storageFilePath, json);
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