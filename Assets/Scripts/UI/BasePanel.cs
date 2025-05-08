using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    protected bool isRemove = false;
    protected new  string name;

    public virtual void OpenPanel(string name)
    {
        this.name = name;
        gameObject.SetActive(true);
    }

    public virtual void ClosePanel(string name)
    {
        isRemove = true;
        gameObject.SetActive(false);
        

        //�Ƴ����棬��ʾ����û��
        if (UIManager.Instance.panelDict.ContainsKey(name))
        {
            UIManager.Instance.panelDict.Remove(name);
        }
    }
}
