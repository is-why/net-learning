# 选项模式（Options Pattern）
- 定义类 `DatabaseSettings`（属性 `ConnectionString`、`Timeout`）
- 在 `ConfigureServices` 中绑定配置：`services.Configure<DatabaseSettings>(configuration.GetSection("Database"))`
- 在服务中注入 `IOptions<DatabaseSettings>`，读取并输出