# 路由与参数绑定
- 实现以下端点（使用最小 API）：
  - `GET /user/{id:int}` → 返回 `{"id": id, "name": "user" + id}`
  - `POST /user` → 从 Body 接收 JSON `{"name": "xxx"}`，返回 201
  - `GET /file` → 从 Query 参数 `?name=test.txt` 返回文件内容（模拟）
- 自定义绑定器，从 Header 中读取 `X-Version` 注入到方法参数