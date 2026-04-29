using DiLifecicle;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ISingleton, MyService>();
builder.Services.AddScoped<IScoped, MyService>();
builder.Services.AddTransient<ITransient, MyService>();

var app = builder.Build();
app.MapGet("/guids", (
    ISingleton singleton1, ISingleton singleton2,
    IScoped scoped1, IScoped scoped2,
    ITransient transient1, ITransient transient2) =>
{
    return new
    {
        Singleton = new { First = singleton1.GetGuid(), Second = singleton2.GetGuid() },
        Scoped = new { First = scoped1.GetGuid(), Second = scoped2.GetGuid() },
        Transient = new { First = transient1.GetGuid(), Second = transient2.GetGuid() }
    };
});

app.Run();
