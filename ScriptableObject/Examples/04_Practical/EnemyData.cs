using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 掉落物品配置
/// </summary>
[System.Serializable]
public class LootItem
{
    public ItemData item;
    [Range(0f, 100f)]
    public float dropChance = 50f;  // 掉落概率
    public int minAmount = 1;
    public int maxAmount = 1;
}

/// <summary>
/// 敌人数据系统
/// 完整的敌人配置
/// </summary>
[CreateAssetMenu(fileName = "NewEnemy", menuName = "Tutorial/04_Practical/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("基础信息")]
    public string enemyName = "敌人";
    public Sprite sprite;

    [TextArea(2, 3)]
    public string description;

    [Header("战斗属性")]
    [Tooltip("最大生命值")]
    public int maxHealth = 100;

    [Tooltip("攻击力")]
    public int attackDamage = 10;

    [Tooltip("防御力")]
    public int defense = 5;

    [Tooltip("移动速度")]
    public float moveSpeed = 3f;

    [Tooltip("攻击范围")]
    public float attackRange = 1.5f;

    [Tooltip("攻击间隔（秒）")]
    public float attackCooldown = 1f;

    [Header("经验与奖励")]
    [Tooltip("击败后获得的经验值")]
    public int experienceReward = 50;

    [Tooltip("击败后获得的金币")]
    public int goldReward = 10;

    [Header("掉落物品")]
    [Tooltip("可能掉落的物品列表")]
    public List<LootItem> lootTable = new List<LootItem>();

    [Header("AI 行为")]
    [Tooltip("视野范围")]
    public float detectionRange = 10f;

    [Tooltip("追击距离")]
    public float chaseDistance = 15f;

    [Tooltip("是否会逃跑")]
    public bool canFlee = false;

    [Tooltip("逃跑生命值百分比")]
    [Range(0f, 1f)]
    public float fleeHealthPercent = 0.2f;

    // 计算实际伤害（考虑防御）
    public int CalculateDamage(int incomingDamage)
    {
        int actualDamage = Mathf.Max(1, incomingDamage - defense);
        return actualDamage;
    }

    // 生成掉落物品
    public List<ItemData> GenerateLoot()
    {
        List<ItemData> droppedItems = new List<ItemData>();

        foreach (var loot in lootTable)
        {
            if (loot.item == null) continue;

            // 随机判断是否掉落
            float roll = Random.Range(0f, 100f);
            if (roll <= loot.dropChance)
            {
                int amount = Random.Range(loot.minAmount, loot.maxAmount + 1);
                for (int i = 0; i < amount; i++)
                {
                    droppedItems.Add(loot.item);
                }
            }
        }

        return droppedItems;
    }

    #if UNITY_EDITOR
    [ContextMenu("显示完整信息")]
    void ShowFullInfo()
    {
        Debug.Log($"=== {enemyName} ===");
        Debug.Log($"生命: {maxHealth} | 攻击: {attackDamage} | 防御: {defense}");
        Debug.Log($"速度: {moveSpeed} | 攻击范围: {attackRange}");
        Debug.Log($"奖励 - 经验: {experienceReward} | 金币: {goldReward}");
        Debug.Log($"掉落物品数量: {lootTable.Count}");
    }

    [ContextMenu("测试掉落")]
    void TestLoot()
    {
        var items = GenerateLoot();
        Debug.Log($"本次掉落了 {items.Count} 个物品:");
        foreach (var item in items)
        {
            Debug.Log($"- {item.itemName}");
        }
    }
    #endif
}
