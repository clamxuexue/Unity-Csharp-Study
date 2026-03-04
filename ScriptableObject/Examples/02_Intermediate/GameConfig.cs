using UnityEngine;

/// <summary>
/// 游戏配置系统
/// 用于存储全局游戏设置
/// </summary>
[CreateAssetMenu(fileName = "GameConfig", menuName = "Tutorial/02_Intermediate/Game Config")]
public class GameConfig : ScriptableObject
{
    [Header("游戏难度")]
    [Range(0.5f, 2f)]
    [Tooltip("敌人伤害倍率")]
    public float enemyDamageMultiplier = 1f;

    [Range(0.5f, 2f)]
    [Tooltip("玩家生命倍率")]
    public float playerHealthMultiplier = 1f;

    [Range(1, 10)]
    [Tooltip("敌人数量倍率")]
    public int enemyCountMultiplier = 1;

    [Header("游戏设置")]
    [Tooltip("主音量")]
    [Range(0f, 1f)]
    public float masterVolume = 1f;

    [Tooltip("音乐音量")]
    [Range(0f, 1f)]
    public float musicVolume = 0.7f;

    [Tooltip("音效音量")]
    [Range(0f, 1f)]
    public float sfxVolume = 0.8f;

    [Header("玩家设置")]
    [Tooltip("鼠标灵敏度")]
    [Range(0.1f, 5f)]
    public float mouseSensitivity = 1f;

    [Tooltip("是否反转 Y 轴")]
    public bool invertYAxis = false;

    [Header("经济系统")]
    [Tooltip("初始金币")]
    public int startingGold = 100;

    [Tooltip("每波奖励金币")]
    public int goldPerWave = 50;

    // 单例模式访问（可选）
    private static GameConfig _instance;
    public static GameConfig Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<GameConfig>("GameConfig");
                if (_instance == null)
                {
                    Debug.LogError("未找到 GameConfig 资源！请在 Resources 文件夹中创建。");
                }
            }
            return _instance;
        }
    }

    // 重置为默认值
    [ContextMenu("重置为默认值")]
    public void ResetToDefaults()
    {
        enemyDamageMultiplier = 1f;
        playerHealthMultiplier = 1f;
        enemyCountMultiplier = 1;
        masterVolume = 1f;
        musicVolume = 0.7f;
        sfxVolume = 0.8f;
        mouseSensitivity = 1f;
        invertYAxis = false;
        startingGold = 100;
        goldPerWave = 50;

        Debug.Log("GameConfig 已重置为默认值");
    }
}
