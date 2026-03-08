using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 07 - Unity基础委托示例
/// 演示委托在Unity中的基本使用方法
///
/// 使用说明：
/// 1. 在Unity中创建一个空GameObject
/// 2. 将此脚本附加到GameObject上
/// 3. 运行游戏查看控制台输出
/// </summary>
public class UnityBasicDelegate : MonoBehaviour
{
    // ========== Unity内置委托类型 ==========

    // UnityAction：Unity的无参数委托（等同于Action）
    private UnityAction simpleAction;

    // UnityAction<T>：Unity的泛型委托
    private UnityAction<string> messageAction;
    private UnityAction<int, float> dataAction;

    // 自定义委托
    public delegate void GameEventDelegate(string eventName);
    private GameEventDelegate gameEvent;

    void Start()
    {
        Debug.Log("========== Unity基础委托示例 ==========\n");

        // ========== 示例1：UnityAction基础用法 ==========
        Debug.Log("【示例1：UnityAction基础用法】");

        // 使用Lambda表达式
        simpleAction = () => Debug.Log("执行简单操作");
        simpleAction.Invoke();

        // 使用方法引用
        simpleAction = PrintHello;
        simpleAction();

        // ========== 示例2：带参数的UnityAction ==========
        Debug.Log("\n【示例2：带参数的UnityAction】");

        messageAction = (msg) => Debug.Log($"消息: {msg}");
        messageAction("Hello Unity!");

        dataAction = (id, value) => Debug.Log($"ID: {id}, 值: {value}");
        dataAction(100, 3.14f);

        // ========== 示例3：委托作为回调 ==========
        Debug.Log("\n【示例3：委托作为回调】");

        // 延迟执行
        DelayedAction(2f, () => Debug.Log("2秒后执行的回调"));

        // 带参数的回调
        LoadData("PlayerData", (data) => Debug.Log($"数据加载完成: {data}"));

        // ========== 示例4：多播委托 ==========
        Debug.Log("\n【示例4：多播委托】");

        gameEvent = OnGameStart;
        gameEvent += OnGameInitialize;
        gameEvent += OnGameLoadAssets;

        Debug.Log("触发游戏启动事件:");
        gameEvent?.Invoke("GameStart");

        // ========== 示例5：委托与协程结合 ==========
        Debug.Log("\n【示例5：委托与协程结合】");

        StartCoroutine(DownloadCoroutine("https://example.com/data", (success) =>
        {
            if (success)
                Debug.Log("✅ 下载成功！");
            else
                Debug.Log("❌ 下载失败！");
        }));

        // ========== 示例6：委托链 ==========
        Debug.Log("\n【示例6：委托链】");

        UnityAction chain = null;
        chain += () => Debug.Log("步骤1: 初始化");
        chain += () => Debug.Log("步骤2: 加载资源");
        chain += () => Debug.Log("步骤3: 启动游戏");

        Debug.Log("执行委托链:");
        chain?.Invoke();

        // ========== 示例7：委托作为参数传递 ==========
        Debug.Log("\n【示例7：委托作为参数传递】");

        ProcessWithCallback("处理数据", () => Debug.Log("处理完成的回调"));

        // ========== 示例8：委托与Update ==========
        Debug.Log("\n【示例8：委托与Update（见Update方法）】");

        // 缓存委托，避免在Update中重复创建
        cachedUpdateAction = () => CheckPlayerInput();
    }

    // 缓存的委托，避免GC
    private UnityAction cachedUpdateAction;
    private float updateTimer = 0f;

    void Update()
    {
        // ❌ 错误做法：每帧创建新委托（会产生GC）
        // SomeMethod(() => DoSomething());

        // ✅ 正确做法：使用缓存的委托
        updateTimer += Time.deltaTime;
        if (updateTimer >= 1f)
        {
            cachedUpdateAction?.Invoke();
            updateTimer = 0f;
        }
    }

    // ========== 辅助方法 ==========

    /// <summary>
    /// 简单的打印方法
    /// </summary>
    void PrintHello()
    {
        Debug.Log("Hello from method!");
    }

    /// <summary>
    /// 延迟执行操作
    /// </summary>
    void DelayedAction(float delay, UnityAction callback)
    {
        StartCoroutine(DelayedActionCoroutine(delay, callback));
    }

    /// <summary>
    /// 延迟执行协程
    /// </summary>
    System.Collections.IEnumerator DelayedActionCoroutine(float delay, UnityAction callback)
    {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
    }

    /// <summary>
    /// 模拟数据加载
    /// </summary>
    void LoadData(string dataName, UnityAction<string> onComplete)
    {
        // 模拟异步加载
        StartCoroutine(LoadDataCoroutine(dataName, onComplete));
    }

    /// <summary>
    /// 数据加载协程
    /// </summary>
    System.Collections.IEnumerator LoadDataCoroutine(string dataName, UnityAction<string> onComplete)
    {
        Debug.Log($"开始加载: {dataName}");
        yield return new WaitForSeconds(1f);
        onComplete?.Invoke(dataName);
    }

    /// <summary>
    /// 游戏启动处理
    /// </summary>
    void OnGameStart(string eventName)
    {
        Debug.Log($"  🎮 [{eventName}] 游戏启动");
    }

    /// <summary>
    /// 游戏初始化处理
    /// </summary>
    void OnGameInitialize(string eventName)
    {
        Debug.Log($"  ⚙️ [{eventName}] 初始化系统");
    }

    /// <summary>
    /// 游戏资源加载处理
    /// </summary>
    void OnGameLoadAssets(string eventName)
    {
        Debug.Log($"  📦 [{eventName}] 加载资源");
    }

    /// <summary>
    /// 模拟下载协程
    /// </summary>
    System.Collections.IEnumerator DownloadCoroutine(string url, UnityAction<bool> onComplete)
    {
        Debug.Log($"开始下载: {url}");
        yield return new WaitForSeconds(1.5f);

        // 模拟成功
        bool success = Random.value > 0.3f;
        onComplete?.Invoke(success);
    }

    /// <summary>
    /// 带回调的处理方法
    /// </summary>
    void ProcessWithCallback(string data, UnityAction callback)
    {
        Debug.Log($"处理中: {data}");
        // 处理完成后调用回调
        callback?.Invoke();
    }

    /// <summary>
    /// 检查玩家输入（在Update中调用）
    /// </summary>
    void CheckPlayerInput()
    {
        // 这里只是示例，实际会检查输入
        // Debug.Log("检查玩家输入...");
    }

    // ========== 示例9：委托与MonoBehaviour生命周期 ==========

    void OnEnable()
    {
        // 在启用时订阅事件
        Debug.Log("OnEnable: 订阅事件");
    }

    void OnDisable()
    {
        // 在禁用时取消订阅事件（防止内存泄漏）
        Debug.Log("OnDisable: 取消订阅事件");

        // 清理委托
        gameEvent = null;
        simpleAction = null;
        messageAction = null;
        dataAction = null;
    }

    void OnDestroy()
    {
        // 在销毁时确保清理所有委托引用
        Debug.Log("OnDestroy: 清理委托引用");
    }
}

/// <summary>
/// 委托管理器示例
/// 演示如何在Unity中管理全局委托
/// </summary>
public class DelegateManager : MonoBehaviour
{
    // 单例模式
    public static DelegateManager Instance { get; private set; }

    // 全局事件委托
    public UnityAction OnGamePaused;
    public UnityAction OnGameResumed;
    public UnityAction<int> OnScoreChanged;
    public UnityAction<string> OnPlayerDied;

    void Awake()
    {
        // 单例初始化
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 暂停游戏
    /// </summary>
    public void PauseGame()
    {
        Time.timeScale = 0f;
        OnGamePaused?.Invoke();
    }

    /// <summary>
    /// 恢复游戏
    /// </summary>
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        OnGameResumed?.Invoke();
    }

    /// <summary>
    /// 更新分数
    /// </summary>
    public void UpdateScore(int newScore)
    {
        OnScoreChanged?.Invoke(newScore);
    }

    /// <summary>
    /// 玩家死亡
    /// </summary>
    public void PlayerDeath(string reason)
    {
        OnPlayerDied?.Invoke(reason);
    }
}

/// <summary>
/// 使用委托管理器的示例
/// </summary>
public class GameController : MonoBehaviour
{
    void Start()
    {
        // 订阅全局事件
        if (DelegateManager.Instance != null)
        {
            DelegateManager.Instance.OnGamePaused += HandleGamePaused;
            DelegateManager.Instance.OnGameResumed += HandleGameResumed;
            DelegateManager.Instance.OnScoreChanged += HandleScoreChanged;
            DelegateManager.Instance.OnPlayerDied += HandlePlayerDied;
        }
    }

    void OnDestroy()
    {
        // 取消订阅（重要！防止内存泄漏）
        if (DelegateManager.Instance != null)
        {
            DelegateManager.Instance.OnGamePaused -= HandleGamePaused;
            DelegateManager.Instance.OnGameResumed -= HandleGameResumed;
            DelegateManager.Instance.OnScoreChanged -= HandleScoreChanged;
            DelegateManager.Instance.OnPlayerDied -= HandlePlayerDied;
        }
    }

    void HandleGamePaused()
    {
        Debug.Log("游戏已暂停");
    }

    void HandleGameResumed()
    {
        Debug.Log("游戏已恢复");
    }

    void HandleScoreChanged(int newScore)
    {
        Debug.Log($"分数更新: {newScore}");
    }

    void HandlePlayerDied(string reason)
    {
        Debug.Log($"玩家死亡: {reason}");
    }
}
