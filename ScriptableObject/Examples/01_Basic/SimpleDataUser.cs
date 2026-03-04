using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 演示如何使用 ScriptableObject 数据
/// </summary>
public class SimpleDataUser : MonoBehaviour
{
    [Header("引用 ScriptableObject 数据")]
    public SimpleData data;

    [Header("UI 组件（可选）")]
    public Text nameText;
    public Text valueText;
    public Text descriptionText;
    public Image iconImage;

    void Start()
    {
        if (data == null)
        {
            Debug.LogError("未分配 SimpleData！");
            return;
        }

        // 在控制台输出数据
        DisplayDataInConsole();

        // 如果有 UI 组件，更新 UI
        UpdateUI();
    }

    void DisplayDataInConsole()
    {
        Debug.Log("=== SimpleData 信息 ===");
        Debug.Log($"物品名称: {data.itemName}");
        Debug.Log($"数值: {data.value}");
        Debug.Log($"描述: {data.description}");
    }

    void UpdateUI()
    {
        if (nameText != null)
            nameText.text = data.itemName;

        if (valueText != null)
            valueText.text = $"数值: {data.value}";

        if (descriptionText != null)
            descriptionText.text = data.description;

        if (iconImage != null && data.icon != null)
            iconImage.sprite = data.icon;
    }

    // 演示：不要在运行时直接修改 ScriptableObject
    [ContextMenu("错误示例：修改 SO 数据")]
    void WrongWayToModify()
    {
        // ❌ 这样做会永久修改资产文件！
        data.value += 10;
        Debug.LogWarning("已修改 ScriptableObject 数据！这会影响所有引用它的对象！");
    }

    // 正确做法：使用本地变量
    private int localValue;

    void Awake()
    {
        if (data != null)
        {
            // ✅ 复制数据到本地变量
            localValue = data.value;
        }
    }

    [ContextMenu("正确示例：修改本地数据")]
    void CorrectWayToModify()
    {
        // ✅ 修改本地副本
        localValue += 10;
        Debug.Log($"本地数值已修改为: {localValue}，原始 SO 数据未改变: {data.value}");
    }
}
