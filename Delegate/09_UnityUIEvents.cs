using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// 09 - Unity UI事件处理示例
/// 演示如何使用委托处理Unity UI事件
///
/// 使用说明：
/// 1. 在Unity中创建Canvas
/// 2. 添加Button、InputField、Toggle、Slider等UI组件
/// 3. 将此脚本附加到Canvas或其他GameObject上
/// 4. 在Inspector中连接UI组件引用
/// 5. 运行游戏测试UI事件
/// </summary>
public class UnityUIEvents : MonoBehaviour
{
    [Header("UI组件引用")]
    public Button startButton;
    public Button pauseButton;
    public Button quitButton;
    public InputField nameInputField;
    public Toggle soundToggle;
    public Slider volumeSlider;
    public Dropdown difficultyDropdown;

    [Header("显示文本")]
    public Text statusText;
    public Text infoText;

    void Start()
    {
        Debug.Log("========== Unity UI事件示例 ==========\n");

        // ========== 示例1：Button点击事件 ==========
        Debug.Log("【示例1：Button点击事件】");

        // 方法1：使用AddListener添加监听器
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
            startButton.onClick.AddListener(() => Debug.Log("Lambda: 开始按钮被点击"));
        }

        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(OnPauseButtonClicked);
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(OnQuitButtonClicked);
        }

        // ========== 示例2：InputField输入事件 ==========
        Debug.Log("【示例2：InputField输入事件】");

        if (nameInputField != null)
        {
            // 输入值改变时触发
            nameInputField.onValueChanged.AddListener(OnNameInputChanged);

            // 输入结束时触发（按回车或失去焦点）
            nameInputField.onEndEdit.AddListener(OnNameInputEndEdit);
        }

        // ========== 示例3：Toggle开关事件 ==========
        Debug.Log("【示例3：Toggle开关事件】");

        if (soundToggle != null)
        {
            soundToggle.onValueChanged.AddListener(OnSoundToggleChanged);
        }

        // ========== 示例4：Slider滑动条事件 ==========
        Debug.Log("【示例4：Slider滑动条事件】");

        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(OnVolumeSliderChanged);
        }

        // ========== 示例5：Dropdown下拉菜单事件 ==========
        Debug.Log("【示例5：Dropdown下拉菜单事件】");

        if (difficultyDropdown != null)
        {
            difficultyDropdown.onValueChanged.AddListener(OnDifficultyChanged);
        }

        UpdateStatusText("UI事件系统已初始化");
    }

    void OnDestroy()
    {
        // ========== 重要：清理事件监听器，防止内存泄漏 ==========
        Debug.Log("清理UI事件监听器");

        if (startButton != null)
        {
            startButton.onClick.RemoveAllListeners();
        }

        if (pauseButton != null)
        {
            pauseButton.onClick.RemoveAllListeners();
        }

        if (quitButton != null)
        {
            quitButton.onClick.RemoveAllListeners();
        }

        if (nameInputField != null)
        {
            nameInputField.onValueChanged.RemoveAllListeners();
            nameInputField.onEndEdit.RemoveAllListeners();
        }

        if (soundToggle != null)
        {
            soundToggle.onValueChanged.RemoveAllListeners();
        }

        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.RemoveAllListeners();
        }

        if (difficultyDropdown != null)
        {
            difficultyDropdown.onValueChanged.RemoveAllListeners();
        }
    }

    // ========== Button事件处理方法 ==========

    /// <summary>
    /// 开始按钮点击处理
    /// </summary>
    void OnStartButtonClicked()
    {
        Debug.Log("🎮 开始游戏");
        UpdateStatusText("游戏已开始");
        UpdateInfoText("欢迎来到游戏世界！");
    }

    /// <summary>
    /// 暂停按钮点击处理
    /// </summary>
    void OnPauseButtonClicked()
    {
        bool isPaused = Time.timeScale == 0f;

        if (isPaused)
        {
            Time.timeScale = 1f;
            Debug.Log("▶️ 游戏继续");
            UpdateStatusText("游戏继续");
        }
        else
        {
            Time.timeScale = 0f;
            Debug.Log("⏸️ 游戏暂停");
            UpdateStatusText("游戏暂停");
        }
    }

    /// <summary>
    /// 退出按钮点击处理
    /// </summary>
    void OnQuitButtonClicked()
    {
        Debug.Log("🚪 退出游戏");
        UpdateStatusText("正在退出...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // ========== InputField事件处理方法 ==========

    /// <summary>
    /// 输入框内容改变时
    /// </summary>
    void OnNameInputChanged(string newValue)
    {
        Debug.Log($"📝 输入内容: {newValue}");
        UpdateInfoText($"当前输入: {newValue}");
    }

    /// <summary>
    /// 输入框编辑结束时
    /// </summary>
    void OnNameInputEndEdit(string finalValue)
    {
        Debug.Log($"✅ 输入完成: {finalValue}");
        UpdateStatusText($"玩家名称: {finalValue}");
    }

    // ========== Toggle事件处理方法 ==========

    /// <summary>
    /// 音效开关改变时
    /// </summary>
    void OnSoundToggleChanged(bool isOn)
    {
        Debug.Log($"🔊 音效: {(isOn ? "开启" : "关闭")}");
        UpdateStatusText($"音效: {(isOn ? "开启" : "关闭")}");

        // 实际项目中这里会控制音效
        AudioListener.volume = isOn ? 1f : 0f;
    }

    // ========== Slider事件处理方法 ==========

    /// <summary>
    /// 音量滑动条改变时
    /// </summary>
    void OnVolumeSliderChanged(float value)
    {
        Debug.Log($"🎵 音量: {value:F2}");
        UpdateInfoText($"音量: {(int)(value * 100)}%");

        // 实际项目中这里会调整音量
        AudioListener.volume = value;
    }

    // ========== Dropdown事件处理方法 ==========

    /// <summary>
    /// 难度下拉菜单改变时
    /// </summary>
    void OnDifficultyChanged(int index)
    {
        string[] difficulties = { "简单", "普通", "困难", "地狱" };
        string difficulty = index < difficulties.Length ? difficulties[index] : "未知";

        Debug.Log($"⚔️ 难度: {difficulty}");
        UpdateStatusText($"难度: {difficulty}");
    }

    // ========== 辅助方法 ==========

    void UpdateStatusText(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
    }

    void UpdateInfoText(string message)
    {
        if (infoText != null)
        {
            infoText.text = message;
        }
    }
}

// ========== 自定义UI事件系统 ==========

/// <summary>
/// 自定义按钮类
/// 演示如何创建自定义UI事件
/// </summary>
public class CustomButton : MonoBehaviour
{
    // 自定义事件
    public UnityEvent OnClick;
    public UnityEvent OnHover;
    public UnityEvent OnExit;

    // 带参数的自定义事件
    [System.Serializable]
    public class ButtonClickedEvent : UnityEvent<string> { }
    public ButtonClickedEvent OnClickWithInfo;

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();

        if (button != null)
        {
            button.onClick.AddListener(HandleClick);
        }
    }

    void HandleClick()
    {
        // 触发自定义事件
        OnClick?.Invoke();
        OnClickWithInfo?.Invoke($"按钮 {gameObject.name} 被点击");
    }

    void OnMouseEnter()
    {
        OnHover?.Invoke();
    }

    void OnMouseExit()
    {
        OnExit?.Invoke();
    }
}

// ========== UI事件管理器 ==========

/// <summary>
/// UI事件管理器
/// 集中管理所有UI事件
/// </summary>
public class UIEventManager : MonoBehaviour
{
    // 定义UI事件委托
    public UnityAction<string> OnButtonClicked;
    public UnityAction<string, string> OnInputSubmitted;
    public UnityAction<bool> OnSettingChanged;

    // 单例
    public static UIEventManager Instance { get; private set; }

    void Awake()
    {
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
    /// 触发按钮点击事件
    /// </summary>
    public void TriggerButtonClick(string buttonName)
    {
        Debug.Log($"[UI事件] 按钮点击: {buttonName}");
        OnButtonClicked?.Invoke(buttonName);
    }

    /// <summary>
    /// 触发输入提交事件
    /// </summary>
    public void TriggerInputSubmit(string fieldName, string value)
    {
        Debug.Log($"[UI事件] 输入提交: {fieldName} = {value}");
        OnInputSubmitted?.Invoke(fieldName, value);
    }

    /// <summary>
    /// 触发设置改变事件
    /// </summary>
    public void TriggerSettingChanged(bool enabled)
    {
        Debug.Log($"[UI事件] 设置改变: {enabled}");
        OnSettingChanged?.Invoke(enabled);
    }
}

// ========== 动态UI创建示例 ==========

/// <summary>
/// 动态UI创建器
/// 演示如何动态创建UI并绑定事件
/// </summary>
public class DynamicUICreator : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform buttonContainer;

    void Start()
    {
        // 动态创建5个按钮
        for (int i = 0; i < 5; i++)
        {
            CreateButton($"按钮 {i + 1}", i);
        }
    }

    /// <summary>
    /// 创建按钮并绑定事件
    /// </summary>
    void CreateButton(string buttonText, int index)
    {
        if (buttonPrefab == null || buttonContainer == null)
            return;

        // 实例化按钮
        GameObject buttonObj = Instantiate(buttonPrefab, buttonContainer);
        Button button = buttonObj.GetComponent<Button>();

        // 设置按钮文本
        Text text = buttonObj.GetComponentInChildren<Text>();
        if (text != null)
        {
            text.text = buttonText;
        }

        // 绑定点击事件（使用Lambda捕获index）
        if (button != null)
        {
            // 注意：这里使用Lambda表达式捕获了index变量
            button.onClick.AddListener(() => OnDynamicButtonClicked(index, buttonText));
        }
    }

    /// <summary>
    /// 动态按钮点击处理
    /// </summary>
    void OnDynamicButtonClicked(int index, string buttonText)
    {
        Debug.Log($"动态按钮被点击: 索引={index}, 文本={buttonText}");
    }
}

// ========== UI事件监听器示例 ==========

/// <summary>
/// UI事件监听器
/// 演示如何监听UI事件管理器的事件
/// </summary>
public class UIEventListener : MonoBehaviour
{
    void OnEnable()
    {
        // 订阅UI事件
        if (UIEventManager.Instance != null)
        {
            UIEventManager.Instance.OnButtonClicked += HandleButtonClicked;
            UIEventManager.Instance.OnInputSubmitted += HandleInputSubmitted;
            UIEventManager.Instance.OnSettingChanged += HandleSettingChanged;
        }
    }

    void OnDisable()
    {
        // 取消订阅
        if (UIEventManager.Instance != null)
        {
            UIEventManager.Instance.OnButtonClicked -= HandleButtonClicked;
            UIEventManager.Instance.OnInputSubmitted -= HandleInputSubmitted;
            UIEventManager.Instance.OnSettingChanged -= HandleSettingChanged;
        }
    }

    void HandleButtonClicked(string buttonName)
    {
        Debug.Log($"[监听器] 收到按钮点击: {buttonName}");
    }

    void HandleInputSubmitted(string fieldName, string value)
    {
        Debug.Log($"[监听器] 收到输入提交: {fieldName} = {value}");
    }

    void HandleSettingChanged(bool enabled)
    {
        Debug.Log($"[监听器] 收到设置改变: {enabled}");
    }
}

// ========== UI按钮辅助类 ==========

/// <summary>
/// UI按钮辅助类
/// 提供便捷的按钮事件绑定方法
/// </summary>
public static class UIButtonHelper
{
    /// <summary>
    /// 为按钮添加点击事件（带延迟防抖）
    /// </summary>
    public static void AddClickListener(this Button button, UnityAction action, float debounceTime = 0.5f)
    {
        if (button == null || action == null)
            return;

        float lastClickTime = 0f;

        button.onClick.AddListener(() =>
        {
            float currentTime = Time.unscaledTime;
            if (currentTime - lastClickTime >= debounceTime)
            {
                lastClickTime = currentTime;
                action.Invoke();
            }
            else
            {
                Debug.Log("点击过快，已忽略");
            }
        });
    }

    /// <summary>
    /// 为按钮添加点击音效
    /// </summary>
    public static void AddClickSound(this Button button, AudioClip clip)
    {
        if (button == null || clip == null)
            return;

        button.onClick.AddListener(() =>
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
        });
    }
}
