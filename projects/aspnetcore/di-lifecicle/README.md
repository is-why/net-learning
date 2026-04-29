# 依赖注入生命周期实战
- 注册三个服务：
  ```csharp
  services.AddTransient<ITransient, MyService>();
  services.AddScoped<IScoped, MyService>();
  services.AddSingleton<ISingleton, MyService>();
  ```
- 每个服务返回一个 GUID
- 端点 `/guids` 在一次请求中调用两次，返回两个 GUID 的 JSON
- 用两个不同浏览器同时请求，观察输出差异