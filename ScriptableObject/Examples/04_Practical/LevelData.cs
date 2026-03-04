using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 敌人波次配置
/// </summary>
[System.Serializable]
public class EnemyWave
{
    public string waveName = "第 1 波";
    public List<EnemyData> enemies = new List<EnemyData>();
    public int enemyCount = 5;
    public float spawnInterval = 2f;  // 生成间隔
    public float delayBeforeWave = 3f;  // 波次前延迟
}

/// <summary>
/// 关卡数据系统
/// </summary>
[CreateAssetMenu(fileName = "NewLevel", menuName = "Tutorial/04_Practical/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("关卡基础信息")]
    public string levelName = "第 1 关";
    public int levelNumber = 1;

    [TextArea(3, 5)]
    public string levelDescription;

    public Sprite levelThumbnail;

    [Header("关卡设置")]
    [Tooltip("关卡时间限制（秒，0 表示无限制）")]
    public float timeLimit = 0f;

    [Tooltip("玩家初始生命值")]
    public int playerStartHealth = 100;

    [Tooltip("关卡难度系数")]
    [Range(0.5f, 3f)]
    public float difficultyMultiplier = 1f;

    [Header("敌人波次")]
    public List<EnemyWave> waves = new List<EnemyWave>();

    [Header("关卡奖励")]
    [Tooltip("完成关卡获得的金币")]
    public int goldReward = 500;

    [Tooltip("完成关卡获得的经验")]
    public int experienceReward = 1000;

    [Tooltip("完成关卡获得的物品")]
    public List<ItemData> rewardItems = new List<ItemData>();

    [Header("星级评价条件")]
    [Tooltip("一星：完成关卡")]
    public bool oneStarRequirement = true;

    [Tooltip("二星：时间限制（秒）")]
    public float twoStarTimeLimit = 300f;

    [Tooltip("三星：不受伤")]
    public bool threeStarNoDamage = false;

    [Header("环境设置")]
    [Tooltip("背景音乐")]
    public AudioClip backgroundMusic;

    [Tooltip("环境预制体")]
    public GameObject environmentPrefab;

    // 获取总敌人数量
    public int GetTotalEnemyCount()
    {
        int total = 0;
        foreach (var wave in waves)
        {
            total += wave.enemyCount;
        }
        return total;
    }

    // 获取关卡总时长估算
    public float GetEstimatedDuration()
    {
        float duration = 0f;
        foreach (var wave in waves)
        {
            duration += wave.delayBeforeWave;
            duration += wave.spawnInterval * wave.enemyCount;
        }
        return duration;
    }

    // 计算星级
    public int CalculateStars(float completionTime, bool tookDamage)
    {
        if (!oneStarRequirement) return 0;

        int stars = 1;  // 基础一星

        // 检查二星条件
        if (twoStarTimeLimit > 0 && completionTime <= twoStarTimeLimit)
        {
            stars = 2;
        }

        // 检查三星条件
        if (threeStarNoDamage && !tookDamage && stars >= 2)
        {
            stars = 3;
        }

        return stars;
    }

    // 获取关卡信息摘要
    public string GetLevelSummary()
    {
        string summary = $"<b>{levelName}</b>\n\n";
        summary += levelDescription + "\n\n";
        summary += $"<color=yellow>波次数量:</color> {waves.Count}\n";
        summary += $"<color=yellow>敌人总数:</color> {GetTotalEnemyCount()}\n";
        summary += $"<color=yellow>预计时长:</color> {GetEstimatedDuration():F0} 秒\n\n";

        summary += "<color=cyan>奖励:</color>\n";
        summary += $"  金币: {goldReward}\n";
        summary += $"  经验: {experienceReward}\n";

        if (rewardItems.Count > 0)
        {
            summary += $"  物品: {rewardItems.Count} 个\n";
        }

        summary += "\n<color=lime>星级条件:</color>\n";
        summary += "  ★ 完成关卡\n";

        if (twoStarTimeLimit > 0)
            summary += $"  ★★ 在 {twoStarTimeLimit} 秒内完成\n";

        if (threeStarNoDamage)
            summary += "  ★★★ 不受伤完成\n";

        return summary;
    }

    #if UNITY_EDITOR
    [ContextMenu("显示关卡信息")]
    void ShowLevelInfo()
    {
        Debug.Log(GetLevelSummary());
    }

    [ContextMenu("验证关卡配置")]
    void ValidateLevel()
    {
        bool isValid = true;

        if (waves.Count == 0)
        {
            Debug.LogError("关卡没有配置任何波次！");
            isValid = false;
        }

        foreach (var wave in waves)
        {
            if (wave.enemies.Count == 0)
            {
                Debug.LogWarning($"波次 '{wave.waveName}' 没有配置敌人！");
                isValid = false;
            }

            if (wave.enemyCount <= 0)
            {
                Debug.LogWarning($"波次 '{wave.waveName}' 敌人数量无效！");
                isValid = false;
            }
        }

        if (isValid)
        {
            Debug.Log($"✓ 关卡 '{levelName}' 配置有效！");
        }
    }

    [ContextMenu("生成测试数据")]
    void GenerateTestData()
    {
        waves.Clear();

        for (int i = 0; i < 3; i++)
        {
            EnemyWave wave = new EnemyWave
            {
                waveName = $"第 {i + 1} 波",
                enemyCount = 5 + i * 2,
                spawnInterval = 2f,
                delayBeforeWave = 5f
            };
            waves.Add(wave);
        }

        Debug.Log("已生成测试波次数据");
    }
    #endif
}
