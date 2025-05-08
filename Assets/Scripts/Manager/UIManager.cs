using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIConst
{
    public const string MercOderPanel = "MercOderPanel";
    public const string RiderSelectPanel = "RiderSelectPanel";
    public const string NewUserPanel = "NewUserPanel";
}
public class UIManager 
{
    private Dictionary<string, string> pathDict;//UI·���ֵ�
    private Dictionary<string, GameObject> prefabDict;//UIԤ�����ֵ�
    public Dictionary<string, BasePanel> panelDict;//�Ѵ򿪽���Ļ����ֵ�
    private static UIManager instance;
    private Transform uiRoot;//UI���ڵ�
     
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new UIManager();
            }
            return instance;
        }
    }

    public Transform UIRoot
    {
        get
        {
            if(uiRoot == null)
            {
                uiRoot = GameObject.Find("Canvas").transform;
            }
            return uiRoot;
        }
    }
    private UIManager()
    {
        InitDicts();
    }

    private void InitDicts()
    {
        prefabDict = new Dictionary<string, GameObject>();
        panelDict = new Dictionary<string, BasePanel>();
        pathDict = new Dictionary<string, string>()
        {
            { UIConst.MercOderPanel, "MercOderPanel" },
            { UIConst.RiderSelectPanel , "RiderSelectPanel" },
            {UIConst.NewUserPanel, "Panel/NewUserPanel" }
        };

    }
    
    //�򿪽���
    public BasePanel OpenPanel(string name)
    {
        BasePanel panel = null;
        //����Ƿ��Ѿ��򿪹�
        if (panelDict.TryGetValue(name, out panel))//�� panelDict �ֵ��в���һ����Ϊ name ����Ŀ�������Խ����Ӧ��ֵ���� panel ����
        {
            Debug.LogError("�Ѿ��򿪹��ý���"+name);
            return null;
        }
        //���·���Ƿ�������
        string path = "";
        if (!pathDict.TryGetValue(name, out path))
        {
            Debug.LogError("�������ƴ���/δ����·��"+name);
            return null;
        }

        //ʹ�û����Ԥ�Ƽ�
        GameObject panelPrefab = null;
        if (!prefabDict.TryGetValue(name, out panelPrefab))
        {
            string realPath = "Prefabs/Panel/" + path;
            panelPrefab = Resources.Load<GameObject>(realPath)as GameObject;
            prefabDict .Add(name, panelPrefab);
        }

        //�򿪽���
        GameObject panelObject = GameObject.Instantiate(panelPrefab, UIRoot, false);
        panel = panelObject.GetComponent<BasePanel>();
        panelDict.Add(name, panel);
        return panel;
    }
    //�رս���
    public bool ClosePanel(string name)
    {
        BasePanel panel = null;
        if (!panelDict.TryGetValue(name, out panel))
        {
            Debug.LogError("����δ��"+name);
            return false;
        }
        panel.ClosePanel(name);
        return true;
    }
}
