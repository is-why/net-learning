# 最小 API + 中间件管道
- 添加三个中间件（`app.Use` 或 `app.UseMiddleware<T>`）：
  1. 打印 "Request start"
  2. 添加响应头 `X-Custom: hello`
  3. 打印 "Request end"
- 实现短路中间件，当路径为 `/secret` 时直接返回 403，不调用后续中间件