# 日志记录
- 添加包：`Microsoft.Extensions.Logging`、`Microsoft.Extensions.Logging.Console`
- 创建 `ILoggerFactory`，添加 Console 提供程序
- 分别输出 `LogInformation`、`LogWarning`、`LogError`
- 用 `Serilog` 将日志同时写入文件（安装 `Serilog.Sinks.File`）