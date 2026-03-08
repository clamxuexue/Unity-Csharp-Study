# Unity ScriptableObject 完整教程

## 目录
1. [什么是 ScriptableObject](#1-什么是-scriptableobject)
2. [基础用法](#2-基础用法)
3. [进阶应用](#3-进阶应用)
4. [实战案例](#4-实战案例)
5. [最佳实践](#5-最佳实践)

---

## 1. 什么是 ScriptableObject

### 1.1 概念介绍
ScriptableObject 是 Unity 提供的一个特殊类，用于创建**可序列化的数据容器**。它独立于场景和 GameObject 存在，可以在项目中作为资产文件保存。
ItemData类是一个“模板”或“蓝图”，而你需要在Unity编辑器中为游戏里每一种不同的物品，创建一个具体的“实例”。

### 1.2 为什么使用 ScriptableObject？

**传统方式的问题：**
```csharp
// 每个敌人 GameObject 都保存一份相同的数据
public class Enemy : MonoBehaviour
{
    public string enemyName = "哥布林";
    public int health = 100;
    public int damage = 10;
    // 100个敌人 = 100份重复数据
}
```

**使用 ScriptableObject 的优势：**
- ✅ **节省内存**：多个对象共享同一份数据
- ✅ **易于管理**：数据与逻辑分离
- ✅ **设计师友好**：可在 Inspector 中直接编辑
- ✅ **运行时不可变**：避免意外修改共享数据

---

## 2. 基础用法

### 2.1 创建最简单的 ScriptableObject

参考代码：`Examples/01_Basic/SimpleData.cs`

```csharp
using UnityEngine;

[CreateAssetMenu(fileName = "NewSimpleData", menuName = "Tutorial/Simple Data")]
public class SimpleData : ScriptableObject
{
    public string itemName;
    public int value;
    public Sprite icon;
}
```

**创建步骤：**
1. 在 Project 窗口右键
2. 选择 `Create > Tutorial > Simple Data`
3. 在 Inspector 中编辑数据

### 2.2 使用 ScriptableObject

参考代码：`Examples/01_Basic/SimpleDataUser.cs`

```csharp
using UnityEngine;

public class SimpleDataUser : MonoBehaviour
{
    public SimpleData data;

    void Start()
    {
        Debug.Log($"物品名称: {data.itemName}");
        Debug.Log($"数值: {data.value}");
    }
}
```

---

## 3. 进阶应用

### 3.1 游戏配置系统

参考代码：`Examples/02_Intermediate/GameConfig.cs`

适用场景：
- 游戏难度设置
- 全局参数配置
- 关卡数据

### 3.2 物品系统

参考代码：`Examples/02_Intermediate/ItemData.cs`

特点：
- 使用枚举定义物品类型
- 支持不同物品属性
- 可扩展的设计

### 3.3 事件系统

参考代码：`Examples/03_Advanced/GameEvent.cs`

优势：
- 解耦代码
- 易于维护
- 支持多个监听者

---

## 4. 实战案例

### 4.1 敌人数据系统

参考代码：`Examples/04_Practical/EnemyData.cs`

完整的敌人配置系统，包含：
- 基础属性（生命、攻击、防御）
- 掉落物品配置
- AI 行为参数

### 4.2 技能系统

参考代码：`Examples/04_Practical/SkillData.cs`

包含：
- 技能基础信息
- 冷却时间管理
- 技能效果配置

### 4.3 关卡系统

参考代码：`Examples/04_Practical/LevelData.cs`

功能：
- 关卡配置
- 敌人波次管理
- 奖励系统

---

## 5. 最佳实践

### 5.1 命名规范
```
✅ 好的命名：
- EnemyData.cs
- WeaponConfig.cs
- GameSettings.cs

❌ 避免：
- Data.cs (太泛化)
- SO_Enemy.cs (不必要的前缀)
```

### 5.2 文件组织
```
Assets/
├── ScriptableObjects/
│   ├── Scripts/          # SO 脚本
│   └── Data/             # SO 资产文件
│       ├── Enemies/
│       ├── Items/
│       └── Levels/
```

### 5.3 注意事项

**⚠️ 运行时修改问题：**
```csharp
// 错误：直接修改会影响所有引用
public void TakeDamage(int damage)
{
    data.health -= damage;  // ❌ 不要这样做！
}

// 正确：使用本地变量
private int currentHealth;

void Start()
{
    currentHealth = data.health;  // ✅ 复制数据
}

public void TakeDamage(int damage)
{
    currentHealth -= damage;  // ✅ 修改本地副本
}
```

**💡 性能优化：**
- 避免在 ScriptableObject 中存储大量运行时数据
- 使用 Resources.Load 时注意缓存
- 考虑使用 Addressables 管理大量资产

**🔧 调试技巧：**
```csharp
#if UNITY_EDITOR
[ContextMenu("Print Data")]
void PrintData()
{
    Debug.Log($"Name: {itemName}, Value: {value}");
}
#endif
```

---

## 6. 常见问题

### Q1: ScriptableObject 和 MonoBehaviour 的区别？
- **MonoBehaviour**：必须附加到 GameObject，随场景存在
- **ScriptableObject**：独立资产，不依赖场景

### Q2: 如何在运行时创建 ScriptableObject？
```csharp
var data = ScriptableObject.CreateInstance<SimpleData>();
data.itemName = "动态创建";
// 注意：运行时创建的 SO 不会保存到磁盘
```

### Q3: 可以在 ScriptableObject 中使用 Awake/Start 吗？
```csharp
// 可以使用 OnEnable
void OnEnable()
{
    Debug.Log("ScriptableObject 被加载");
}
```

---

## 7. 学习路径建议

1. **初学者**：从 `01_Basic` 开始，理解基本概念
2. **进阶**：学习 `02_Intermediate`，掌握常见应用
3. **高级**：研究 `03_Advanced`，理解事件系统
4. **实战**：参考 `04_Practical`，构建完整系统

---

## 8. 扩展阅读

- Unity 官方文档：[ScriptableObject](https://docs.unity3d.com/Manual/class-ScriptableObject.html)
- 推荐视频：Unity 官方教程系列
- 设计模式：数据驱动设计

---

**祝学习愉快！🎮**
