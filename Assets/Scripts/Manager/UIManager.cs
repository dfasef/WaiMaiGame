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
    private Dictionary<string, string> pathDict;//UI路径字典
    private Dictionary<string, GameObject> prefabDict;//UI预制体字典
    public Dictionary<string, BasePanel> panelDict;//已打开界面的缓存字典

    // UI对象池
    private Dictionary<string, Stack<BasePanel>> panelPool = new Dictionary<string, Stack<BasePanel>>();
    private static UIManager instance;
    private Transform uiRoot;//UI根节点
     
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
    
    //打开界面
    public BasePanel OpenPanel(string name)
    {
        BasePanel panel = null;
        //检查是否已经打开过
        if (panelDict.TryGetValue(name, out panel))
        {
            panel.gameObject.SetActive(true);
            panel.OpenPanel(name);
            return panel;
        }
        // 2. 尝试从对象池获取
        if (TryGetFromPool(name, out panel))
        {
            panelDict.Add(name, panel);
            panel.gameObject.SetActive(true);
            panel.OpenPanel(name);
            return panel;
        }

        // 3. 加载新实例
        string path;
        if (!pathDict.TryGetValue(name, out path))
        {
            Debug.LogError($"未配置路径: {name}");
            return null;
        }

        GameObject prefab;
        if (!prefabDict.TryGetValue(name, out prefab))
        {
            prefab = Resources.Load<GameObject>($"Prefabs/Panel/{path}");
            if (prefab == null)
            {
                Debug.LogError($"加载失败: Prefabs/Panel/{path}");
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
    //关闭界面
    // 关闭界面（修改后的逻辑）
    public bool ClosePanel(string name)
    {
        if (!panelDict.TryGetValue(name, out BasePanel panel))
        {
            Debug.LogError($"界面未打开: {name}");
            return false;
        }

        // 移出激活列表
        panelDict.Remove(name);

        // 存入对象池
        if (!panelPool.ContainsKey(name))
            panelPool[name] = new Stack<BasePanel>();
        panelPool[name].Push(panel);

        // 执行关闭操作
        panel.gameObject.SetActive(false);
        panel.ClosePanel(name);
        return true;
    }
    // 从对象池获取面板
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

