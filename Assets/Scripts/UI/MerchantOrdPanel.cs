using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;


public class MerchantOrdPanel:MonoBehaviour
{
    //关闭订单界面,可优化为直接关闭面板
    public void CloseOrderPanel()
    {
        this.gameObject.SetActive(false);
    }

    [Header("UI配置")]
    public Transform orderListParent; // 订单UI列表容器（建议使用Vertical Layout Group）
    public GameObject orderItemPrefab; // 订单项预制件

    [Header("数据")]
    public List<Order> visibleOrders = new List<Order>();// 显示在面板上的订单列表
    public string currentMerchantID; // 当前商家ID
 
    public static MerchantOrdPanel Instance { get; private set; }
   
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else 
        {
            Instance = this;
        }
       
    }

    public GameObject MerchantOrdPanelUI;
    
    public void ShowMerchantOrdPanel(string merchantID)
    {
        Debug.Log("2");
        currentMerchantID = merchantID;
        // 当UI激活时刷新列表
        RefreshOrderList();
        // 注册订单更新事件
        GetMerchantManager().OnOrderChanged += HandleOrderUpdate;
    }

    
    MerOrderManager GetMerchantManager()
    {
        // 获取商家管理器
        MerOrderManager merchantOrderManager = OrderManager.Instance.
            GetMerOrderManager(currentMerchantID);
        return merchantOrderManager;
    }
    void OnDisable()
    {
        // 注销事件
        if (GetMerchantManager() != null)
            GetMerchantManager().OnOrderChanged -= HandleOrderUpdate;
    }

    // 刷新整个订单列表
    public void RefreshOrderList()
    {
        // 清空现有UI
        foreach (Transform child in orderListParent)
            Destroy(child.gameObject);
        // 获取所有活跃订单
        var orders = GetMerchantManager().GetAllOrders();

        // 生成新的UI项
        foreach (var order in orders)
        {
            CreateOrderUIItem(order);
        }
    }

    // 创建单个订单UI项
    void CreateOrderUIItem(Order order)
    {
       
        GameObject item = Instantiate(orderItemPrefab, orderListParent);
        OrderUIItem uiItem = item.GetComponent<OrderUIItem>();
        uiItem.Initialize(order);
    }

    // 订单更新事件处理
    void HandleOrderUpdate(Order updatedOrder)
    {
        RefreshOrderList();
        // 查找对应UI项更新状态
        foreach (Transform child in orderListParent)
        {
            OrderUIItem uiItem = child.GetComponent<OrderUIItem>();
            if (uiItem.OrderID == updatedOrder.orderID)
            {
                uiItem.UpdateOrderStatus(updatedOrder.status);//更新该UI项状态
                break;
            }
        }
    }
}


