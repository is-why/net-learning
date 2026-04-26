# 配置读取（不用 Web）
- 添加 NuGet 包：`Microsoft.Extensions.Configuration`、`Microsoft.Extensions.Configuration.Json`
- 创建 `appsettings.json`，内容如 `{"DbConnection": "server=.;database=MyDB", "Timeout": 30}`
- 用 `ConfigurationBuilder` 加载 JSON，读取配置并输出
- 支持环境变量覆盖（例如设置 `MyApp__DbConnection`）