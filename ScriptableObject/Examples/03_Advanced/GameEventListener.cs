using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 游戏事件监听器
/// 附加到 GameObject 上，监听 GameEvent
/// </summary>
public class GameEventListener : MonoBehaviour
{
    [Header("要监听的事件")]
    [Tooltip("拖入一个 GameEvent ScriptableObject")]
    public GameEvent gameEvent;

    [Header("事件响应")]
    [Tooltip("当事件触发时执行的操作")]
    public UnityEvent response;

    private void OnEnable()
    {
        if (gameEvent != null)
        {
            gameEvent.RegisterListener(this);
        }
    }

    private void OnDisable()
    {
        if (gameEvent != null)
        {
            gameEvent.UnregisterListener(this);
        }
    }

    public void OnEventRaised()
    {
        response?.Invoke();
    }

    // 测试方法
    [ContextMenu("测试响应")]
    void TestResponse()
    {
        OnEventRaised();
    }
}
