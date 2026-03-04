using UnityEngine;

/// <summary>
/// 最简单的 ScriptableObject 示例
/// 用于存储基础数据
/// </summary>
[CreateAssetMenu(fileName = "NewSimpleData", menuName = "Tutorial/01_Basic/Simple Data")]
public class SimpleData : ScriptableObject
{
    [Header("基础信息")]
    [Tooltip("物品名称")]
    public string itemName;

    [Tooltip("数值")]
    public int value;

    [Tooltip("图标")]
    public Sprite icon;

    [Header("描述")]
    [TextArea(3, 5)]
    public string description;

    // 在编辑器中右键菜单显示数据
    #if UNITY_EDITOR
    [ContextMenu("打印数据")]
    void PrintData()
    {
        Debug.Log($"物品名称: {itemName}");
        Debug.Log($"数值: {value}");
        Debug.Log($"描述: {description}");
    }
    #endif
}
