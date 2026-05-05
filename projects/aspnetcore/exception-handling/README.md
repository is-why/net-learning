# 全局异常处理中间件
- 编写 `ExceptionHandlingMiddleware`，捕获所有未处理异常
- 返回标准 JSON：`{"error": "Internal server error"}`
- 根据环境变量（`ASPNETCORE_ENVIRONMENT`）决定是否返回详细堆栈
- 访问 `/throw` 时得到友好 JSON 错误。
