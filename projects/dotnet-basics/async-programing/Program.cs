using System.Diagnostics;

var urls = new[]
{
    "https://www.baidu.com",
    "https://www.bing.com",
    "https://www.sogou.com"
};

// Synchronous execution
Console.WriteLine("Synchronous execution:");
var stopwatch = Stopwatch.StartNew();
stopwatch.Start();
try
{
    foreach (var url in urls)
    {
        using var httpClient = new HttpClient();
        var content = httpClient.GetStringAsync(url).GetAwaiter().GetResult();
        Console.WriteLine($"Downloaded {url}, length: {content.Length}");
    }
}
catch (Exception)
{
    Console.WriteLine("An error occurred during synchronous execution.");
}
stopwatch.Stop();
Console.WriteLine($"Total time: {stopwatch.ElapsedMilliseconds}ms");

// Asynchronous execution
Console.WriteLine("\nAsynchronous execution:");
stopwatch.Restart();
var tasks = urls.Select(async url =>
    {
        try
        {
            using var httpClient = new HttpClient();
            var content = await httpClient.GetStringAsync(url);
            Console.WriteLine($"Downloaded {url}, length: {content.Length}");
        }
        catch (Exception)
        {
            Console.WriteLine($"An error occurred while downloading {url}.");
        }
    });
await Task.WhenAll(tasks);
stopwatch.Stop();
Console.WriteLine($"Total time: {stopwatch.ElapsedMilliseconds}ms");

// Cancellation with timeout
var urlsWithDelay = new[]
{
    "https://google.com",
    "https://httpbin.org/delay/5", // This will delay for 5 seconds
    "https://github.com"
};
var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(3));
Console.WriteLine("\nAsynchronous execution with cancellation:");
stopwatch.Restart();
var timeoutTasks = urlsWithDelay.Select(async url =>
    {
        try
        {
            using var httpClient = new HttpClient();
            var content = await httpClient.GetStringAsync(url, cancellationTokenSource.Token);
            Console.WriteLine($"Downloaded {url}, length: {content.Length}");
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine($"Download of {url} was canceled due to timeout.");
        }
        catch (Exception)
        {
            Console.WriteLine($"An error occurred while downloading {url}.");
        }
    });
await Task.WhenAll(timeoutTasks);
stopwatch.Stop();
Console.WriteLine($"Total time with cancellation: {stopwatch.ElapsedMilliseconds}ms");