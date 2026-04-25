var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Custom middleware to block access to the "/secret" path
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/secret")
    {
        context.Response.StatusCode = 403;
        return;
    }
    await next(context);
});

// Custom middleware to log the request and response
app.Use(async (context, next) =>
{
    Console.WriteLine("Request start");
    await next(context);
});

// Custom middleware to add a custom header to the response
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Custom", "Hello");
    await next(context);
});

// Custom middleware to log the end of the request
app.Use(async (context, next) =>
{
    Console.WriteLine("Request end");
    await next(context);
});

app.MapGet("/hello", () => "Hello World!");
app.Run();