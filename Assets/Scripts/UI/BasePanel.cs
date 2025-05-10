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
        // ��ӳ�ʼ���߼���������ˢ�£�
    }

    public virtual void ClosePanel(string name)
    {
        gameObject.SetActive(false);
        // ע�⣺���ٲ��� UIManager ���ֵ�
    }
}
