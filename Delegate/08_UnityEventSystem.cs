using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

/// <summary>
/// 08 - Unity游戏事件系统示例
/// 演示如何使用委托构建完整的游戏事件系统
///
/// 使用说明：
/// 1. 在Unity中创建一个空GameObject命名为"EventSystem"
/// 2. 将EventManager脚本附加到该GameObject上
/// 3. 创建其他GameObject并附加相应的监听器脚本
/// 4. 运行游戏测试事件系统
/// </summary>

// ========== 事件参数类 ==========

/// <summary>
/// 玩家事件参数
/// </summary>
public class PlayerEventArgs
{
    public string PlayerName { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public Vector3 Position { get; set; }
}

/// <summary>
/// 敌人事件参数
/// </summary>
public class EnemyEventArgs
{
    public string EnemyType { get; set; }
    public int Damage { get; set; }
    public Vector3 SpawnPosition { get; set; }
}

/// <summary>
/// 物品事件参数
/// </summary>
public class ItemEventArgs
{
    public string ItemName { get; set; }
    public int ItemID { get; set; }
    public int Quantity { get; set; }
}

/// <summary>
/// 游戏状态事件参数
/// </summary>
public class GameStateEventArgs
{
    public string StateName { get; set; }
    public float StateTime { get; set; }
}

// ========== 事件管理器（单例模式） ==========

/// <summary>
/// 全局事件管理器
/// 使用单例模式管理游戏中的所有事件
/// </summary>
public class EventManager : MonoBehaviour
{
    // 单例实例
    private static EventManager instance;
    public static EventManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("EventManager");
                instance = go.AddComponent<EventManager>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    // ========== 玩家相关事件 ==========
    public UnityAction<PlayerEventArgs> OnPlayerSpawned;
    public UnityAction<PlayerEventArgs> OnPlayerDamaged;
    public UnityAction<PlayerEventArgs> OnPlayerHealed;
    public UnityAction<PlayerEventArgs> OnPlayerDied;
    public UnityAction<int> OnPlayerLevelUp;
    public UnityAction<int, int> OnPlayerScoreChanged; // 旧分数, 新分数

    // ========== 敌人相关事件 ==========
    public UnityAction<EnemyEventArgs> OnEnemySpawned;
    public UnityAction<EnemyEventArgs> OnEnemyDied;
    public UnityAction OnAllEnemiesDefeated;

    // ========== 物品相关事件 ==========
    public UnityAction<ItemEventArgs> OnItemCollected;
    public UnityAction<ItemEventArgs> OnItemUsed;
    public UnityAction<ItemEventArgs> OnItemDropped;

    // ========== 游戏状态事件 ==========
    public UnityAction OnGameStarted;
    public UnityAction OnGamePaused;
    public UnityAction OnGameResumed;
    public UnityAction<int> OnGameOver; // 最终分数
    public UnityAction<GameStateEventArgs> OnGameStateChanged;

    // ========== UI相关事件 ==========
    public UnityAction<string> OnShowMessage;
    public UnityAction<string, float> OnShowNotification; // 消息, 持续时间

    void Awake()
    {
        // 确保只有一个实例
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ========== 触发事件的便捷方法 ==========

    public void TriggerPlayerSpawned(PlayerEventArgs args)
    {
        Debug.Log($"[事件] 玩家生成: {args.PlayerName}");
        OnPlayerSpawned?.Invoke(args);
    }

    public void TriggerPlayerDamaged(PlayerEventArgs args)
    {
        Debug.Log($"[事件] 玩家受伤: {args.PlayerName}, 剩余生命: {args.Health}");
        OnPlayerDamaged?.Invoke(args);
    }

    public void TriggerPlayerHealed(PlayerEventArgs args)
    {
        Debug.Log($"[事件] 玩家治疗: {args.PlayerName}, 当前生命: {args.Health}");
        OnPlayerHealed?.Invoke(args);
    }

    public void TriggerPlayerDied(PlayerEventArgs args)
    {
        Debug.Log($"[事件] 玩家死亡: {args.PlayerName}");
        OnPlayerDied?.Invoke(args);
    }

    public void TriggerEnemySpawned(EnemyEventArgs args)
    {
        Debug.Log($"[事件] 敌人生成: {args.EnemyType}");
        OnEnemySpawned?.Invoke(args);
    }

    public void TriggerEnemyDied(EnemyEventArgs args)
    {
        Debug.Log($"[事件] 敌人死亡: {args.EnemyType}");
        OnEnemyDied?.Invoke(args);
    }

    public void TriggerItemCollected(ItemEventArgs args)
    {
        Debug.Log($"[事件] 收集物品: {args.ItemName} x{args.Quantity}");
        OnItemCollected?.Invoke(args);
    }

    public void TriggerGameStarted()
    {
        Debug.Log("[事件] 游戏开始");
        OnGameStarted?.Invoke();
    }

    public void TriggerGameOver(int finalScore)
    {
        Debug.Log($"[事件] 游戏结束, 最终分数: {finalScore}");
        OnGameOver?.Invoke(finalScore);
    }

    public void TriggerShowMessage(string message)
    {
        OnShowMessage?.Invoke(message);
    }
}

// ========== 玩家控制器 ==========

/// <summary>
/// 玩家控制器
/// 演示如何触发玩家相关事件
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("玩家属性")]
    public string playerName = "勇士";
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;

        // 触发玩家生成事件
        EventManager.Instance.TriggerPlayerSpawned(new PlayerEventArgs
        {
            PlayerName = playerName,
            Health = currentHealth,
            MaxHealth = maxHealth,
            Position = transform.position
        });
    }

    void Update()
    {
        // 测试：按键触发事件
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(20);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            Heal(15);
        }
    }

    /// <summary>
    /// 受到伤害
    /// </summary>
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);

        // 触发受伤事件
        EventManager.Instance.TriggerPlayerDamaged(new PlayerEventArgs
        {
            PlayerName = playerName,
            Health = currentHealth,
            MaxHealth = maxHealth,
            Position = transform.position
        });

        // 检查是否死亡
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// 治疗
    /// </summary>
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(maxHealth, currentHealth);

        // 触发治疗事件
        EventManager.Instance.TriggerPlayerHealed(new PlayerEventArgs
        {
            PlayerName = playerName,
            Health = currentHealth,
            MaxHealth = maxHealth,
            Position = transform.position
        });
    }

    /// <summary>
    /// 死亡
    /// </summary>
    void Die()
    {
        // 触发死亡事件
        EventManager.Instance.TriggerPlayerDied(new PlayerEventArgs
        {
            PlayerName = playerName,
            Health = 0,
            MaxHealth = maxHealth,
            Position = transform.position
        });
    }
}

// ========== UI管理器 ==========

/// <summary>
/// UI管理器
/// 监听游戏事件并更新UI
/// </summary>
public class UIManager : MonoBehaviour
{
    void OnEnable()
    {
        // 订阅事件
        EventManager.Instance.OnPlayerDamaged += HandlePlayerDamaged;
        EventManager.Instance.OnPlayerHealed += HandlePlayerHealed;
        EventManager.Instance.OnPlayerDied += HandlePlayerDied;
        EventManager.Instance.OnItemCollected += HandleItemCollected;
        EventManager.Instance.OnGameStarted += HandleGameStarted;
        EventManager.Instance.OnGameOver += HandleGameOver;
        EventManager.Instance.OnShowMessage += HandleShowMessage;
    }

    void OnDisable()
    {
        // 取消订阅（重要！防止内存泄漏）
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnPlayerDamaged -= HandlePlayerDamaged;
            EventManager.Instance.OnPlayerHealed -= HandlePlayerHealed;
            EventManager.Instance.OnPlayerDied -= HandlePlayerDied;
            EventManager.Instance.OnItemCollected -= HandleItemCollected;
            EventManager.Instance.OnGameStarted -= HandleGameStarted;
            EventManager.Instance.OnGameOver -= HandleGameOver;
            EventManager.Instance.OnShowMessage -= HandleShowMessage;
        }
    }

    // ========== 事件处理方法 ==========

    void HandlePlayerDamaged(PlayerEventArgs args)
    {
        Debug.Log($"[UI] 更新生命条: {args.Health}/{args.MaxHealth}");
        // 实际项目中这里会更新UI元素
        // healthBar.value = (float)args.Health / args.MaxHealth;
    }

    void HandlePlayerHealed(PlayerEventArgs args)
    {
        Debug.Log($"[UI] 显示治疗效果: +{args.Health}");
        // 显示治疗特效
    }

    void HandlePlayerDied(PlayerEventArgs args)
    {
        Debug.Log($"[UI] 显示死亡界面");
        // 显示游戏结束界面
    }

    void HandleItemCollected(ItemEventArgs args)
    {
        Debug.Log($"[UI] 显示物品提示: 获得 {args.ItemName} x{args.Quantity}");
        // 显示物品获得提示
    }

    void HandleGameStarted()
    {
        Debug.Log($"[UI] 隐藏主菜单，显示游戏UI");
    }

    void HandleGameOver(int finalScore)
    {
        Debug.Log($"[UI] 显示游戏结束界面，分数: {finalScore}");
    }

    void HandleShowMessage(string message)
    {
        Debug.Log($"[UI] 显示消息: {message}");
    }
}

// ========== 音效管理器 ==========

/// <summary>
/// 音效管理器
/// 监听游戏事件并播放相应音效
/// </summary>
public class AudioManager : MonoBehaviour
{
    void OnEnable()
    {
        // 订阅事件
        EventManager.Instance.OnPlayerDamaged += HandlePlayerDamaged;
        EventManager.Instance.OnPlayerDied += HandlePlayerDied;
        EventManager.Instance.OnEnemyDied += HandleEnemyDied;
        EventManager.Instance.OnItemCollected += HandleItemCollected;
    }

    void OnDisable()
    {
        // 取消订阅
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnPlayerDamaged -= HandlePlayerDamaged;
            EventManager.Instance.OnPlayerDied -= HandlePlayerDied;
            EventManager.Instance.OnEnemyDied -= HandleEnemyDied;
            EventManager.Instance.OnItemCollected -= HandleItemCollected;
        }
    }

    void HandlePlayerDamaged(PlayerEventArgs args)
    {
        Debug.Log($"[音效] 播放受伤音效");
        // AudioSource.PlayOneShot(damageSound);
    }

    void HandlePlayerDied(PlayerEventArgs args)
    {
        Debug.Log($"[音效] 播放死亡音效");
        // AudioSource.PlayOneShot(deathSound);
    }

    void HandleEnemyDied(EnemyEventArgs args)
    {
        Debug.Log($"[音效] 播放敌人死亡音效");
        // AudioSource.PlayOneShot(enemyDeathSound);
    }

    void HandleItemCollected(ItemEventArgs args)
    {
        Debug.Log($"[音效] 播放物品收集音效");
        // AudioSource.PlayOneShot(collectSound);
    }
}

// ========== 成就系统 ==========

/// <summary>
/// 成就系统
/// 监听游戏事件并解锁成就
/// </summary>
public class AchievementSystem : MonoBehaviour
{
    private int enemiesKilled = 0;
    private int itemsCollected = 0;

    void OnEnable()
    {
        EventManager.Instance.OnEnemyDied += HandleEnemyDied;
        EventManager.Instance.OnItemCollected += HandleItemCollected;
        EventManager.Instance.OnPlayerLevelUp += HandlePlayerLevelUp;
    }

    void OnDisable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnEnemyDied -= HandleEnemyDied;
            EventManager.Instance.OnItemCollected -= HandleItemCollected;
            EventManager.Instance.OnPlayerLevelUp -= HandlePlayerLevelUp;
        }
    }

    void HandleEnemyDied(EnemyEventArgs args)
    {
        enemiesKilled++;
        Debug.Log($"[成就] 击败敌人数: {enemiesKilled}");

        // 检查成就
        if (enemiesKilled == 10)
        {
            UnlockAchievement("新手猎人");
        }
        else if (enemiesKilled == 100)
        {
            UnlockAchievement("资深猎人");
        }
    }

    void HandleItemCollected(ItemEventArgs args)
    {
        itemsCollected += args.Quantity;
        Debug.Log($"[成就] 收集物品数: {itemsCollected}");

        if (itemsCollected >= 50)
        {
            UnlockAchievement("收藏家");
        }
    }

    void HandlePlayerLevelUp(int newLevel)
    {
        Debug.Log($"[成就] 玩家升级到 {newLevel} 级");

        if (newLevel == 10)
        {
            UnlockAchievement("初出茅庐");
        }
        else if (newLevel == 50)
        {
            UnlockAchievement("传奇英雄");
        }
    }

    void UnlockAchievement(string achievementName)
    {
        Debug.Log($"🏆 [成就解锁] {achievementName}");
        EventManager.Instance.TriggerShowMessage($"解锁成就: {achievementName}");
    }
}

// ========== 游戏测试控制器 ==========

/// <summary>
/// 测试控制器
/// 用于测试事件系统
/// </summary>
public class EventSystemTester : MonoBehaviour
{
    void Start()
    {
        Debug.Log("========== Unity事件系统测试 ==========");
        Debug.Log("按键说明:");
        Debug.Log("H - 玩家受伤");
        Debug.Log("J - 玩家治疗");
        Debug.Log("K - 生成敌人");
        Debug.Log("L - 收集物品");
        Debug.Log("Space - 开始游戏");
        Debug.Log("Esc - 游戏结束");
    }

    void Update()
    {
        // 测试各种事件
        if (Input.GetKeyDown(KeyCode.K))
        {
            EventManager.Instance.TriggerEnemySpawned(new EnemyEventArgs
            {
                EnemyType = "哥布林",
                Damage = 10,
                SpawnPosition = Vector3.zero
            });
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            EventManager.Instance.TriggerItemCollected(new ItemEventArgs
            {
                ItemName = "生命药水",
                ItemID = 1001,
                Quantity = 1
            });
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            EventManager.Instance.TriggerGameStarted();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EventManager.Instance.TriggerGameOver(9999);
        }
    }
}
