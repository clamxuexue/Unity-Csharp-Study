using UnityEngine;

/// <summary>
/// 游戏事件触发器
/// 用于在代码中触发事件
/// </summary>
public class GameEventTrigger : MonoBehaviour
{
    [Header("要触发的事件")]
    public GameEvent eventToTrigger;

    [Header("触发设置")]
    [Tooltip("在 Start 时自动触发")]
    public bool triggerOnStart = false;

    [Tooltip("延迟触发时间（秒）")]
    public float delayTime = 0f;

    void Start()
    {
        if (triggerOnStart)
        {
            TriggerEvent();
        }
    }

    public void TriggerEvent()
    {
        if (eventToTrigger == null)
        {
            Debug.LogWarning("未分配 GameEvent！");
            return;
        }

        if (delayTime > 0)
        {
            Invoke(nameof(TriggerEventInternal), delayTime);
        }
        else
        {
            TriggerEventInternal();
        }
    }

    private void TriggerEventInternal()
    {
        eventToTrigger.Raise();
        Debug.Log($"触发了事件: {eventToTrigger.name}");
    }

    // 可以从 UI 按钮调用
    [ContextMenu("立即触发事件")]
    public void TriggerEventImmediately()
    {
        if (eventToTrigger != null)
        {
            eventToTrigger.Raise();
        }
    }
}
