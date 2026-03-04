using UnityEngine;

/// <summary>
/// 物品类型枚举
/// </summary>
public enum ItemType
{
    Weapon,      // 武器
    Armor,       // 护甲
    Consumable,  // 消耗品
    QuestItem,   // 任务物品
    Material     // 材料
}

/// <summary>
/// 物品稀有度
/// </summary>
public enum ItemRarity
{
    Common,      // 普通（白色）
    Uncommon,    // 非凡（绿色）
    Rare,        // 稀有（蓝色）
    Epic,        // 史诗（紫色）
    Legendary    // 传说（橙色）
}

/// <summary>
/// 物品数据系统
/// 展示更复杂的 ScriptableObject 应用
/// </summary>
[CreateAssetMenu(fileName = "NewItem", menuName = "Tutorial/02_Intermediate/Item Data")]
public class ItemData : ScriptableObject
{
    [Header("基础信息")]
    public string itemName = "新物品";
    public ItemType itemType = ItemType.Consumable;
    public ItemRarity rarity = ItemRarity.Common;

    [TextArea(2, 4)]
    public string description = "物品描述";

    public Sprite icon;

    [Header("属性")]
    [Tooltip("物品等级")]
    [Range(1, 100)]
    public int level = 1;

    [Tooltip("最大堆叠数量")]
    public int maxStackSize = 99;

    [Tooltip("购买价格")]
    public int buyPrice = 100;

    [Tooltip("出售价格")]
    public int sellPrice = 50;

    [Header("效果（根据类型不同）")]
    [Tooltip("攻击力加成")]
    public int attackBonus = 0;

    [Tooltip("防御力加成")]
    public int defenseBonus = 0;

    [Tooltip("生命值恢复")]
    public int healthRestore = 0;

    [Tooltip("魔法值恢复")]
    public int manaRestore = 0;

    // 获取稀有度颜色
    public Color GetRarityColor()
    {
        switch (rarity)
        {
            case ItemRarity.Common:
                return Color.white;
            case ItemRarity.Uncommon:
                return Color.green;
            case ItemRarity.Rare:
                return Color.blue;
            case ItemRarity.Epic:
                return new Color(0.64f, 0.21f, 0.93f); // 紫色
            case ItemRarity.Legendary:
                return new Color(1f, 0.5f, 0f); // 橙色
            default:
                return Color.white;
        }
    }

    // 获取物品完整描述
    public string GetFullDescription()
    {
        string fullDesc = $"<b>{itemName}</b>\n";
        fullDesc += $"类型: {GetItemTypeText()}\n";
        fullDesc += $"稀有度: {GetRarityText()}\n";
        fullDesc += $"等级: {level}\n\n";
        fullDesc += description + "\n\n";

        // 根据类型显示不同属性
        if (attackBonus > 0)
            fullDesc += $"攻击力 +{attackBonus}\n";
        if (defenseBonus > 0)
            fullDesc += $"防御力 +{defenseBonus}\n";
        if (healthRestore > 0)
            fullDesc += $"恢复生命 +{healthRestore}\n";
        if (manaRestore > 0)
            fullDesc += $"恢复魔法 +{manaRestore}\n";

        fullDesc += $"\n购买价格: {buyPrice} 金币";
        fullDesc += $"\n出售价格: {sellPrice} 金币";

        return fullDesc;
    }

    private string GetItemTypeText()
    {
        switch (itemType)
        {
            case ItemType.Weapon: return "武器";
            case ItemType.Armor: return "护甲";
            case ItemType.Consumable: return "消耗品";
            case ItemType.QuestItem: return "任务物品";
            case ItemType.Material: return "材料";
            default: return "未知";
        }
    }

    private string GetRarityText()
    {
        switch (rarity)
        {
            case ItemRarity.Common: return "普通";
            case ItemRarity.Uncommon: return "非凡";
            case ItemRarity.Rare: return "稀有";
            case ItemRarity.Epic: return "史诗";
            case ItemRarity.Legendary: return "传说";
            default: return "未知";
        }
    }

    #if UNITY_EDITOR
    [ContextMenu("打印完整信息")]
    void PrintFullInfo()
    {
        Debug.Log(GetFullDescription());
    }
    #endif
}
