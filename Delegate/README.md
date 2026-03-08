# C# 委托（Delegate）完整教程

## 目录
1. [什么是委托](#什么是委托)
2. [基础语法](#基础语法)
3. [委托的声明和使用](#委托的声明和使用)
4. [多播委托](#多播委托)
5. [内置委托类型](#内置委托类型)
6. [匿名方法和Lambda表达式](#匿名方法和lambda表达式)
7. [事件（Event）](#事件event)
8. [Unity中的委托应用](#unity中的委托应用)
9. [高级用法](#高级用法)
10. [最佳实践](#最佳实践)

---

## 什么是委托

委托（Delegate）是C#中的一种引用类型，它可以封装方法的引用。简单来说，委托就像是方法的"指针"，允许你将方法作为参数传递，或者存储方法以便稍后调用。

### 委托的特点
- **类型安全**：委托只能引用与其签名匹配的方法
- **多播**：一个委托可以引用多个方法
- **灵活性**：支持回调、事件处理等设计模式

---

## 基础语法

### 委托的声明语法
```csharp
// 语法：delegate 返回类型 委托名(参数列表);
delegate void MyDelegate(string message);
delegate int CalculateDelegate(int a, int b);
```

### 示例代码
参见：`01_BasicDelegate.cs`

---

## 委托的声明和使用

### 基本步骤
1. **声明委托类型**
2. **创建委托实例**
3. **调用委托**

### 示例代码
参见：`02_DelegateUsage.cs`

---

## 多播委托

多播委托可以引用多个方法，调用时会按顺序执行所有方法。

### 关键操作符
- `+=`：添加方法到委托链
- `-=`：从委托链中移除方法

### 示例代码
参见：`03_MulticastDelegate.cs`

---

## 内置委托类型

C#提供了三种常用的内置委托类型，无需自己声明：

### 1. Action
- 无返回值的委托
- 可以有0-16个参数
```csharp
Action<string> action = (msg) => Console.WriteLine(msg);
```

### 2. Func
- 有返回值的委托
- 最后一个类型参数是返回值类型
```csharp
Func<int, int, int> func = (a, b) => a + b;
```

### 3. Predicate
- 返回bool的委托
- 用于条件判断
```csharp
Predicate<int> predicate = (x) => x > 0;
```

### 示例代码
参见：`04_BuiltInDelegates.cs`

---

## 匿名方法和Lambda表达式

### 匿名方法（C# 2.0）
```csharp
delegate(参数) { 方法体 }
```

### Lambda表达式（C# 3.0+）
```csharp
(参数) => 表达式
(参数) => { 语句块 }
```

### 示例代码
参见：`05_LambdaExpressions.cs`

---

## 事件（Event）

事件是基于委托的特殊封装，用于实现发布-订阅模式。

### 事件 vs 委托
- 事件只能在声明类内部触发
- 事件提供了更好的封装性
- 事件使用`event`关键字

### 示例代码
参见：`06_Events.cs`

---

## Unity中的委托应用

### 常见应用场景
1. **UnityAction**：Unity的内置委托类型
2. **UI事件处理**：按钮点击、输入事件等
3. **游戏事件系统**：角色死亡、得分变化等
4. **协程回调**：异步操作完成通知
5. **自定义事件管理器**

### 示例代码
参见：
- `07_UnityBasicDelegate.cs`
- `08_UnityEventSystem.cs`
- `09_UnityUIEvents.cs`

---

## 高级用法

### 1. 委托协变和逆变
- **协变（Covariance）**：返回值类型可以更具体
- **逆变（Contravariance）**：参数类型可以更通用

### 2. 委托缓存
避免在频繁调用的地方创建委托实例（如Update方法）

### 3. 委托链的返回值
多播委托只返回最后一个方法的返回值

### 4. 异步委托
使用BeginInvoke和EndInvoke进行异步调用

### 示例代码
参见：`10_AdvancedDelegates.cs`

---

## 最佳实践

### ✅ 推荐做法
1. **使用内置委托**：优先使用Action、Func而非自定义委托
2. **事件封装**：对外暴露使用event，内部使用委托
3. **空检查**：调用前检查委托是否为null
   ```csharp
   myDelegate?.Invoke();
   ```
4. **避免内存泄漏**：及时取消订阅事件
5. **命名规范**：
   - 委托类型：以`Delegate`或`Handler`结尾
   - 事件：使用动词或动词短语

### ❌ 避免的做法
1. 在Update等高频方法中创建委托实例
2. 忘记取消事件订阅导致内存泄漏
3. 过度使用委托导致代码难以追踪
4. 在多线程环境下不加锁直接操作委托

---

## 代码文件说明

| 文件名 | 说明 |
|--------|------|
| `01_BasicDelegate.cs` | 委托基础语法 |
| `02_DelegateUsage.cs` | 委托的声明和使用 |
| `03_MulticastDelegate.cs` | 多播委托 |
| `04_BuiltInDelegates.cs` | 内置委托类型 |
| `05_LambdaExpressions.cs` | Lambda表达式 |
| `06_Events.cs` | 事件系统 |
| `07_UnityBasicDelegate.cs` | Unity基础委托 |
| `08_UnityEventSystem.cs` | Unity游戏事件系统 |
| `09_UnityUIEvents.cs` | Unity UI事件处理 |
| `10_AdvancedDelegates.cs` | 高级用法 |

---

## 学习路径建议

1. **初学者**：按顺序学习01-06
2. **Unity开发者**：重点学习07-09
3. **进阶开发者**：深入研究10和最佳实践

---

## 运行说明

### 纯C#示例（01-06, 10）
```bash
# 使用.NET CLI运行
dotnet run --project <文件名>

# 或直接编译运行
csc <文件名>.cs
./<文件名>.exe
```

### Unity示例（07-09）
1. 在Unity中创建新项目
2. 将脚本文件复制到Assets/Scripts目录
3. 按照脚本中的注释进行场景设置
4. 运行查看效果

---

## 参考资源

- [Microsoft C# 委托文档](https://docs.microsoft.com/zh-cn/dotnet/csharp/programming-guide/delegates/)
- [Unity UnityAction文档](https://docs.unity3d.com/ScriptReference/Events.UnityAction.html)
- [C# 事件文档](https://docs.microsoft.com/zh-cn/dotnet/csharp/programming-guide/events/)

---

**作者**：Claude
**日期**：2026-03-08
**版本**：1.0
