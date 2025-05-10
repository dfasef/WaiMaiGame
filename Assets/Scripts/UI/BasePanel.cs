using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    private string panelName;

    public virtual void OpenPanel(string name)
    {
        panelName = name;
        gameObject.SetActive(true);
        // 添加初始化逻辑（如数据刷新）
    }

    public virtual void ClosePanel(string name)
    {
        gameObject.SetActive(false);
        // 注意：不再操作 UIManager 的字典
    }
}
