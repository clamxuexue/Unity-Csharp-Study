using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 技能系统管理器
/// 演示如何使用 SkillData
/// </summary>
public class SkillSystem : MonoBehaviour
{
    [Header("角色技能列表")]
    public List<SkillData> skills = new List<SkillData>();

    // 运行时冷却追踪（不修改 SO）
    private Dictionary<SkillData, float> skillCooldowns = new Dictionary<SkillData, float>();

    // 当前魔法值
    private int currentMana = 100;
    private int maxMana = 100;

    void Update()
    {
        // 更新冷却时间
        UpdateCooldowns();

        // 测试：按数字键使用技能
        if (Input.GetKeyDown(KeyCode.Alpha1)) UseSkill(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) UseSkill(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) UseSkill(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) UseSkill(3);
    }

    void UpdateCooldowns()
    {
        List<SkillData> keysToUpdate = new List<SkillData>(skillCooldowns.Keys);
        foreach (var skill in keysToUpdate)
        {
            if (skillCooldowns[skill] > 0)
            {
                skillCooldowns[skill] -= Time.deltaTime;
            }
        }
    }

    public void UseSkill(int index)
    {
        if (index < 0 || index >= skills.Count)
        {
            Debug.LogWarning("技能索引无效！");
            return;
        }

        SkillData skill = skills[index];
        if (skill == null)
        {
            Debug.LogWarning("技能数据为空！");
            return;
        }

        // 检查是否可以使用
        if (!CanUseSkill(skill))
        {
            return;
        }

        // 使用技能
        ExecuteSkill(skill);

        // 消耗魔法
        currentMana -= skill.manaCost;

        // 设置冷却
        skillCooldowns[skill] = skill.cooldownTime;

        Debug.Log($"使用技能: {skill.skillName}，剩余魔法: {currentMana}");
    }

    bool CanUseSkill(SkillData skill)
    {
        // 检查魔法值
        if (currentMana < skill.manaCost)
        {
            Debug.Log($"魔法值不足！需要 {skill.manaCost}，当前 {currentMana}");
            return false;
        }

        // 检查冷却
        if (skillCooldowns.ContainsKey(skill) && skillCooldowns[skill] > 0)
        {
            Debug.Log($"技能冷却中，剩余 {skillCooldowns[skill]:F1} 秒");
            return false;
        }

        return true;
    }

    void ExecuteSkill(SkillData skill)
    {
        float value = skill.GetCurrentValue();

        switch (skill.effectType)
        {
            case SkillEffectType.Damage:
                DealDamage(skill, value);
                break;

            case SkillEffectType.Heal:
                Heal(skill, value);
                break;

            case SkillEffectType.Buff:
                ApplyBuff(skill, value);
                break;

            case SkillEffectType.Debuff:
                ApplyDebuff(skill, value);
                break;

            default:
                Debug.Log($"执行技能: {skill.skillName}");
                break;
        }

        // 播放特效
        if (skill.effectPrefab != null)
        {
            Instantiate(skill.effectPrefab, transform.position, Quaternion.identity);
        }

        // 播放音效
        if (skill.castSound != null)
        {
            AudioSource.PlayClipAtPoint(skill.castSound, transform.position);
        }
    }

    void DealDamage(SkillData skill, float damage)
    {
        Debug.Log($"造成 {damage} 点伤害！目标: {skill.targetType}");

        // 根据目标类型处理
        switch (skill.targetType)
        {
            case SkillTargetType.SingleEnemy:
                // 查找最近的敌人
                GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
                if (enemy != null)
                {
                    enemy.GetComponent<Enemy>()?.TakeDamage((int)damage);
                }
                break;

            case SkillTargetType.AllEnemies:
                // 对所有敌人造成伤害
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (var e in enemies)
                {
                    e.GetComponent<Enemy>()?.TakeDamage((int)damage);
                }
                break;

            case SkillTargetType.Area:
                // 区域伤害
                Collider[] hits = Physics.OverlapSphere(transform.position, skill.effectRadius);
                foreach (var hit in hits)
                {
                    if (hit.CompareTag("Enemy"))
                    {
                        hit.GetComponent<Enemy>()?.TakeDamage((int)damage);
                    }
                }
                break;
        }
    }

    void Heal(SkillData skill, float healAmount)
    {
        Debug.Log($"治疗 {healAmount} 点生命值");
        // 实际项目中调用角色的治疗方法
    }

    void ApplyBuff(SkillData skill, float value)
    {
        Debug.Log($"施加增益效果，持续 {skill.duration} 秒");
        // 实际项目中应用增益效果
    }

    void ApplyDebuff(SkillData skill, float value)
    {
        Debug.Log($"施加减益效果，持续 {skill.duration} 秒");
        // 实际项目中应用减益效果
    }

    // 获取技能剩余冷却时间
    public float GetSkillCooldown(SkillData skill)
    {
        if (skillCooldowns.ContainsKey(skill))
        {
            return Mathf.Max(0, skillCooldowns[skill]);
        }
        return 0;
    }

    // 恢复魔法值
    void RegenerateMana()
    {
        if (currentMana < maxMana)
        {
            currentMana = Mathf.Min(maxMana, currentMana + 1);
        }
    }

    void Start()
    {
        // 每秒恢复魔法
        InvokeRepeating(nameof(RegenerateMana), 1f, 1f);
    }

    #if UNITY_EDITOR
    [ContextMenu("显示所有技能")]
    void ShowAllSkills()
    {
        Debug.Log($"=== 技能列表 ({skills.Count}) ===");
        for (int i = 0; i < skills.Count; i++)
        {
            if (skills[i] != null)
            {
                Debug.Log($"{i + 1}. {skills[i].skillName} - {skills[i].description}");
            }
        }
    }
    #endif
}
