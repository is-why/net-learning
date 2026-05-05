var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapGet("/throw", () =>
{
    throw new Exception("This is a test exception");
});

app.Run();

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _env;

    public ExceptionHandlingMiddleware(RequestDelegate next, IHostEnvironment env)
    {
        _next = next;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        var response = new { error = "Internal server error" };

        if (_env.IsDevelopment())
        {
            var detailResponse = new
            {
                error = "Internal server error",
                message = ex.Message,
                stackTrace = ex.StackTrace
            };
            await context.Response.WriteAsJsonAsync(detailResponse);
        }
        else
        {
            await context.Response.WriteAsJsonAsync(response);
        }

    }
}