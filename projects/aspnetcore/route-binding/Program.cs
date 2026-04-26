var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/user/{id:int}", (int id) => new { id, name = "user" + id });

app.MapPost("/user", async (HttpContext context) =>
{
    var user = await context.Request.ReadFromJsonAsync<User>();
    if (user == null)
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsJsonAsync(new { message = "Invalid user data" });
        return;
    }
    context.Response.StatusCode = 201;
    await context.Response.WriteAsJsonAsync(new { message = $"User '{user.Name}' created successfully" });
});

app.MapGet("/file", (string name) => new { fileName = name, content = $"This is simulated content of {name}" });

app.MapGet("/version", (HttpContext context) =>
{
    var version = context.Request.Headers["X-Version"].FirstOrDefault();
    return Results.Ok(new { version = version ?? "unknown" });
});

app.Run();

record User(string Name);