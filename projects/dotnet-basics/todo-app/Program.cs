using TodoApp;
using TodoApp.Services;

// Initialize the DI container and register services
var container = new DIContainer();
container.Register<ITodoService, TodoService>();

// Resolve the ITodoService from the container
var todoService = container.GetService<ITodoService>();
