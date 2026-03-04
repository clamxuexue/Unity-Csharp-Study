using UnityEngine;

/// <summary>
/// 敌人控制器
/// 使用 EnemyData 的实际应用示例
/// </summary>
public class Enemy : MonoBehaviour
{
    [Header("敌人数据")]
    public EnemyData enemyData;

    [Header("运行时数据（不要直接修改 SO）")]
    private int currentHealth;
    private float lastAttackTime;
    private bool isDead = false;

    // 引用
    private Transform player;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        if (enemyData == null)
        {
            Debug.LogError("未分配 EnemyData！");
            enabled = false;
            return;
        }

        // ✅ 从 ScriptableObject 复制数据到本地变量
        currentHealth = enemyData.maxHealth;

        // 设置精灵
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && enemyData.sprite != null)
        {
            spriteRenderer.sprite = enemyData.sprite;
        }

        // 查找玩家（简化示例）
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        Debug.Log($"{enemyData.enemyName} 生成，生命值: {currentHealth}");
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // AI 行为
        if (distanceToPlayer <= enemyData.detectionRange)
        {
            // 检查是否需要逃跑
            if (enemyData.canFlee && GetHealthPercent() <= enemyData.fleeHealthPercent)
            {
                Flee();
            }
            else if (distanceToPlayer <= enemyData.attackRange)
            {
                Attack();
            }
            else if (distanceToPlayer <= enemyData.chaseDistance)
            {
                Chase();
            }
        }
    }

    void Chase()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * enemyData.moveSpeed * Time.deltaTime;
    }

    void Flee()
    {
        Vector3 direction = (transform.position - player.position).normalized;
        transform.position += direction * enemyData.moveSpeed * 1.5f * Time.deltaTime;
    }

    void Attack()
    {
        if (Time.time - lastAttackTime >= enemyData.attackCooldown)
        {
            lastAttackTime = Time.time;
            Debug.Log($"{enemyData.enemyName} 攻击玩家，造成 {enemyData.attackDamage} 点伤害！");

            // 这里可以调用玩家的受伤方法
            // player.GetComponent<Player>()?.TakeDamage(enemyData.attackDamage);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        // 使用 EnemyData 的方法计算实际伤害
        int actualDamage = enemyData.CalculateDamage(damage);
        currentHealth -= actualDamage;

        Debug.Log($"{enemyData.enemyName} 受到 {actualDamage} 点伤害，剩余生命: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log($"{enemyData.enemyName} 被击败！");

        // 给予奖励
        GiveRewards();

        // 生成掉落
        GenerateLoot();

        // 销毁对象（实际项目中可能播放死亡动画）
        Destroy(gameObject, 0.5f);
    }

    void GiveRewards()
    {
        Debug.Log($"获得 {enemyData.experienceReward} 经验值");
        Debug.Log($"获得 {enemyData.goldReward} 金币");

        // 这里可以调用游戏管理器给予奖励
        // GameManager.Instance.AddExperience(enemyData.experienceReward);
        // GameManager.Instance.AddGold(enemyData.goldReward);
    }

    void GenerateLoot()
    {
        var loot = enemyData.GenerateLoot();
        if (loot.Count > 0)
        {
            Debug.Log($"掉落了 {loot.Count} 个物品:");
            foreach (var item in loot)
            {
                Debug.Log($"- {item.itemName}");
                // 实际项目中在这里生成掉落物品对象
            }
        }
    }

    float GetHealthPercent()
    {
        return (float)currentHealth / enemyData.maxHealth;
    }

    // 在编辑器中可视化检测范围
    void OnDrawGizmosSelected()
    {
        if (enemyData == null) return;

        // 检测范围
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, enemyData.detectionRange);

        // 攻击范围
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyData.attackRange);

        // 追击范围
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, enemyData.chaseDistance);
    }
}
