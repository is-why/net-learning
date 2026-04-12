using SimpleDIContainer;

DIContainer container = new DIContainer();

// Singleton example
container.Register<IGreetingService, HelloService>(ServiceLifeTime.Singleton);
var greetingService1 = container.GetService<IGreetingService>();
var greetingService2 = container.GetService<IGreetingService>();
greetingService1.Greet();
greetingService2.Greet();
Console.WriteLine($"Singleton: Are both instances the same? {ReferenceEquals(greetingService1, greetingService2)}");

// Transient example
container.Register<IGreetingService, HelloService>(ServiceLifeTime.Transient);
var greetingService3 = container.GetService<IGreetingService>();
var greetingService4 = container.GetService<IGreetingService>();
greetingService3.Greet();
greetingService4.Greet();
Console.WriteLine($"Transient: Are both instances the same? {ReferenceEquals(greetingService3, greetingService4)}");