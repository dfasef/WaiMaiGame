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
        if (panelDict.TryGetValue(name, out panel))//从 panelDict 字典中查找一个键为 name 的条目，并尝试将其对应的值赋给 panel 变量
        {
            Debug.LogError("已经打开过该界面"+name);
            return null;
        }
        //检查路径是否有配置
        string path = "";
        if (!pathDict.TryGetValue(name, out path))
        {
            Debug.LogError("界面名称错误/未配置路径"+name);
            return null;
        }

        //使用缓存的预制件
        GameObject panelPrefab = null;
        if (!prefabDict.TryGetValue(name, out panelPrefab))
        {
            string realPath = "Prefabs/Panel/" + path;
            panelPrefab = Resources.Load<GameObject>(realPath)as GameObject;
            prefabDict .Add(name, panelPrefab);
        }

        //打开界面
        GameObject panelObject = GameObject.Instantiate(panelPrefab, UIRoot, false);
        panel = panelObject.GetComponent<BasePanel>();
        panelDict.Add(name, panel);
        return panel;
    }
    //关闭界面
    public bool ClosePanel(string name)
    {
        BasePanel panel = null;
        if (!panelDict.TryGetValue(name, out panel))
        {
            Debug.LogError("界面未打开"+name);
            return false;
        }
        panel.ClosePanel(name);
        return true;
    }
}
