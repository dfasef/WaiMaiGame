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

    // UI�����
    private Dictionary<string, Stack<BasePanel>> panelPool = new Dictionary<string, Stack<BasePanel>>();
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
        if (panelDict.TryGetValue(name, out panel))
        {
            panel.gameObject.SetActive(true);
            panel.OpenPanel(name);
            return panel;
        }
        // 2. ���ԴӶ���ػ�ȡ
        if (TryGetFromPool(name, out panel))
        {
            panelDict.Add(name, panel);
            panel.gameObject.SetActive(true);
            panel.OpenPanel(name);
            return panel;
        }

        // 3. ������ʵ��
        string path;
        if (!pathDict.TryGetValue(name, out path))
        {
            Debug.LogError($"δ����·��: {name}");
            return null;
        }

        GameObject prefab;
        if (!prefabDict.TryGetValue(name, out prefab))
        {
            prefab = Resources.Load<GameObject>($"Prefabs/Panel/{path}");
            if (prefab == null)
            {
                Debug.LogError($"����ʧ��: Prefabs/Panel/{path}");
                return null;
            }
            prefabDict.Add(name, prefab);
        }
        GameObject panelObj = GameObject.Instantiate(prefab, UIRoot, false);
        panel = panelObj.GetComponent<BasePanel>();
        panelDict.Add(name, panel);
        panel.OpenPanel(name);
        return panel;
    }
    //�رս���
    // �رս��棨�޸ĺ���߼���
    public bool ClosePanel(string name)
    {
        if (!panelDict.TryGetValue(name, out BasePanel panel))
        {
            Debug.LogError($"����δ��: {name}");
            return false;
        }

        // �Ƴ������б�
        panelDict.Remove(name);

        // ��������
        if (!panelPool.ContainsKey(name))
            panelPool[name] = new Stack<BasePanel>();
        panelPool[name].Push(panel);

        // ִ�йرղ���
        panel.gameObject.SetActive(false);
        panel.ClosePanel(name);
        return true;
    }
    // �Ӷ���ػ�ȡ���
    private bool TryGetFromPool(string name, out BasePanel panel)
    {
        panel = null;
        if (panelPool.TryGetValue(name, out Stack<BasePanel> pool) && pool.Count > 0)
        {
            panel = pool.Pop();
            return true;
        }
        return false;
    }
}

