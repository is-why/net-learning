# 异步编程实战
- 添加 NuGet 包：`Microsoft.Extensions.Configuration`、`Microsoft.Extensions.Configuration.Json`
- 创建 `appsettings.json`，内容如 `{"Database": "MyAppDb","Timeout": 30,"Ports": [70,80,90]}`
- 用 `ConfigurationBuilder` 加载 JSON，读取配置并输出
- 支持环境变量覆盖（例如设置 `Database`）