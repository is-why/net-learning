namespace SimpleDIContainer
{
    public class HelloService : IGreetingService
    {
        private readonly string _instanceID = Guid.NewGuid().ToString();

        public void Greet()
        {
            Console.WriteLine($"Hello from HelloService! Instance ID: {_instanceID}");
        }
    }
}