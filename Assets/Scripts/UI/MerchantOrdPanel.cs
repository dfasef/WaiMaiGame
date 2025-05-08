using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MerchantOrdPanel:BasePanel
{
    //关闭订单界面,可优化为直接关闭面板
    public void CloseOrderPanel()
    {
        ClosePanel(UIConst.MercOderPanel);
    }
    [Header("UI配置")]
    public Transform orderListParent; // UI列表容器（建议使用Vertical Layout Group）
    public GameObject orderItemPrefab; // 订单项预制件

    [Header("数据")]
    public List<Order> visibleOrders = new List<Order>();

    void OnEnable()
    {
        // 当UI激活时刷新列表
        RefreshOrderList();

        // 注册订单更新事件
        OrderManager.Instance.OnOrderChanged += HandleOrderUpdate;
    }

    void OnDisable()
    {
        // 注销事件
        if (OrderManager.Instance != null)
            OrderManager.Instance.OnOrderChanged -= HandleOrderUpdate;
    }

    // 刷新整个订单列表
    public void RefreshOrderList()
    {
        // 清空现有UI
        foreach (Transform child in orderListParent)
            Destroy(child.gameObject);

        // 获取所有活跃订单
        var orders = OrderManager.Instance.GetAllOrders();

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

        uiItem.Initialize(order, () =>
        {
            // 接单按钮回调
            OrderManager.Instance.AcceptOrder(order.orderID);
        });
    }

    // 订单更新事件处理
    void HandleOrderUpdate(Order updatedOrder)
    {
        RefreshOrderList();
        // 查找对应UI项更新状态
        foreach (Transform child in orderListParent)
        {
            OrderUIItem item = child.GetComponent<OrderUIItem>();
            if (item.OrderID == updatedOrder.orderID)
            {
                item.UpdateStatus(updatedOrder.status);
                break;
            }
        }
    }
}


