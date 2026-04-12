# 手写简易依赖注入容器

## 任务
- 定义接口 `IGreetingService` 和实现 `HelloService`
- 实现 `DIContainer` 类，支持 `Register<TInterface, TImplementation>()` 和 `GetService<T>()`
- 分别实现 **Transient**（每次新建）和 **Singleton**（缓存实例）两种生命周期
- 在 `Main` 中演示：多次获取 `IGreetingService`，观察是否为同一实例

## 收获
- **DI 容器**本质是一个字典，存储接口类型与实现类型/实例的映射关系
- **Transient**：每次获取都创建新实例
- **Singleton**：全局共享同一个实例