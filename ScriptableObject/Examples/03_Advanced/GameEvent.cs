using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 游戏事件系统 - ScriptableObject 作为事件通道
/// 实现解耦的事件通信
/// </summary>
[CreateAssetMenu(fileName = "NewGameEvent", menuName = "Tutorial/03_Advanced/Game Event")]
public class GameEvent : ScriptableObject
{
    // 监听器列表
    private List<GameEventListener> listeners = new List<GameEventListener>();

    // 触发事件
    public void Raise()
    {
        // 从后向前遍历，避免在遍历时移除元素导致问题
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            if (listeners[i] != null)
            {
                listeners[i].OnEventRaised();
            }
        }

        #if UNITY_EDITOR
        Debug.Log($"事件 '{name}' 被触发，通知了 {listeners.Count} 个监听器");
        #endif
    }

    // 注册监听器
    public void RegisterListener(GameEventListener listener)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }

    // 注销监听器
    public void UnregisterListener(GameEventListener listener)
    {
        if (listeners.Contains(listener))
        {
            listeners.Remove(listener);
        }
    }

    #if UNITY_EDITOR
    [ContextMenu("触发事件（测试）")]
    void RaiseForTest()
    {
        Raise();
    }

    [ContextMenu("显示监听器数量")]
    void ShowListenerCount()
    {
        Debug.Log($"事件 '{name}' 当前有 {listeners.Count} 个监听器");
    }
    #endif
}
