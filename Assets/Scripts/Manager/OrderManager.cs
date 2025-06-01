using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private Dictionary<string, MerOrderManager> merchantDict = new Dictionary<string, MerOrderManager>();

    //接受商家注册订单管理器
    public void AddMerchantOrderManager(string merchantID, MerOrderManager manager)
    {
        if (!merchantDict.ContainsKey(merchantID))
        {
            merchantDict[merchantID] = manager;
        }
        else
        {
            Debug.LogError($"商家 {merchantID} 已经注册了 MerOrderManager");
        }
    }


    //通过商家Id获取对应的订单管理器
    public MerOrderManager GetMerOrderManager(string merchantID)
    {
        if (merchantDict.TryGetValue(merchantID, out MerOrderManager manager))
        {
            return manager;
        }
        Debug.LogError($"找不到商家{merchantID}对应的MerOrderManager");
        return null;
    }

}
