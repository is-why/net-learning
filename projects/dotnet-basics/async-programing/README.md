# 异步编程实战
- 用 `HttpClient` 同步下载三个网页，记录总耗时
- 改为 `await Task.WhenAll` 异步下载，对比时间
- 加入 `CancellationToken`，实现 3 秒超时取消（其中一个网址故意延迟 >3s）
- 观察异常处理（`OperationCanceledException`）
