using UnityEngine;

/// <summary>
/// 技能效果类型
/// </summary>
public enum SkillEffectType
{
    Damage,          // 伤害
    Heal,            // 治疗
    Buff,            // 增益
    Debuff,          // 减益
    Summon,          // 召唤
    Teleport         // 传送
}

/// <summary>
/// 技能目标类型
/// </summary>
public enum SkillTargetType
{
    Self,            // 自身
    SingleEnemy,     // 单个敌人
    AllEnemies,      // 所有敌人
    SingleAlly,      // 单个友军
    AllAllies,       // 所有友军
    Area             // 区域
}

/// <summary>
/// 技能数据系统
/// </summary>
[CreateAssetMenu(fileName = "NewSkill", menuName = "Tutorial/04_Practical/Skill Data")]
public class SkillData : ScriptableObject
{
    [Header("基础信息")]
    public string skillName = "新技能";
    public Sprite icon;

    [TextArea(2, 4)]
    public string description;

    [Header("技能类型")]
    public SkillEffectType effectType = SkillEffectType.Damage;
    public SkillTargetType targetType = SkillTargetType.SingleEnemy;

    [Header("技能参数")]
    [Tooltip("技能等级")]
    [Range(1, 10)]
    public int level = 1;

    [Tooltip("冷却时间（秒）")]
    public float cooldownTime = 5f;

    [Tooltip("消耗魔法值")]
    public int manaCost = 20;

    [Tooltip("施法时间（秒）")]
    public float castTime = 0f;

    [Header("效果数值")]
    [Tooltip("基础数值（伤害/治疗量等）")]
    public float baseValue = 50f;

    [Tooltip("每级成长")]
    public float valuePerLevel = 10f;

    [Tooltip("效果范围（如果是区域技能）")]
    public float effectRadius = 5f;

    [Tooltip("持续时间（如果是持续效果）")]
    public float duration = 0f;

    [Header("视觉效果")]
    [Tooltip("技能特效预制体")]
    public GameObject effectPrefab;

    [Tooltip("施法动画名称")]
    public string animationName = "Cast";

    [Header("音效")]
    public AudioClip castSound;
    public AudioClip hitSound;

    // 计算当前等级的技能数值
    public float GetCurrentValue()
    {
        return baseValue + (valuePerLevel * (level - 1));
    }

    // 获取技能完整描述
    public string GetFullDescription()
    {
        string desc = $"<b>{skillName}</b> (等级 {level})\n\n";
        desc += description + "\n\n";

        desc += $"<color=yellow>效果类型:</color> {GetEffectTypeText()}\n";
        desc += $"<color=yellow>目标:</color> {GetTargetTypeText()}\n\n";

        desc += $"<color=cyan>数值:</color> {GetCurrentValue()}\n";

        if (effectRadius > 0 && targetType == SkillTargetType.Area)
            desc += $"<color=cyan>范围:</color> {effectRadius}m\n";

        if (duration > 0)
            desc += $"<color=cyan>持续时间:</color> {duration}秒\n";

        desc += $"\n<color=red>魔法消耗:</color> {manaCost}\n";
        desc += $"<color=red>冷却时间:</color> {cooldownTime}秒\n";

        if (castTime > 0)
            desc += $"<color=red>施法时间:</color> {castTime}秒";

        return desc;
    }

    private string GetEffectTypeText()
    {
        switch (effectType)
        {
            case SkillEffectType.Damage: return "伤害";
            case SkillEffectType.Heal: return "治疗";
            case SkillEffectType.Buff: return "增益";
            case SkillEffectType.Debuff: return "减益";
            case SkillEffectType.Summon: return "召唤";
            case SkillEffectType.Teleport: return "传送";
            default: return "未知";
        }
    }

    private string GetTargetTypeText()
    {
        switch (targetType)
        {
            case SkillTargetType.Self: return "自身";
            case SkillTargetType.SingleEnemy: return "单个敌人";
            case SkillTargetType.AllEnemies: return "所有敌人";
            case SkillTargetType.SingleAlly: return "单个友军";
            case SkillTargetType.AllAllies: return "所有友军";
            case SkillTargetType.Area: return "区域";
            default: return "未知";
        }
    }

    #if UNITY_EDITOR
    [ContextMenu("显示技能信息")]
    void ShowSkillInfo()
    {
        Debug.Log(GetFullDescription());
    }

    [ContextMenu("升级技能")]
    void LevelUp()
    {
        if (level < 10)
        {
            level++;
            Debug.Log($"{skillName} 升级到 {level} 级！当前数值: {GetCurrentValue()}");
        }
        else
        {
            Debug.Log("技能已达到最高等级！");
        }
    }
    #endif
}
