# 配置热更新与多环境
- 创建 `appsettings.Development.json` 和 `appsettings.Production.json`，各含 `"EnvName": "Dev"` / `"Prod"`
- 端点 `/env` 返回当前环境名称
- 使用 `IConfiguration` 直接读，再使用 `IOptionsMonitor` 读，对比修改 JSON 后是否热更新
- 修改 JSON 不重启，`/env` 返回新值。