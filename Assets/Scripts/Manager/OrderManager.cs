using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    

    public static OrderManager Instance { get; private set; }

    public delegate void OrderUpdateHandler(Order updatedOrder);
    public event OrderUpdateHandler OnOrderChanged;
   
    public List<Order> activeOrders = new List<Order>();//进行中订单列表
    
    private List<Order> completedOrders = new List<Order>();// 已完成的订单列表
    public List<Order> GetAllOrders() => new List<Order>(activeOrders);//返回OrderManager类中的所有订单
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddOrder(Order newOrder)// 新增订单方法
    {
        activeOrders.Add(newOrder);
        OnOrderChanged?.Invoke(newOrder);
    }

    public void AcceptOrder(string orderID)/// 被骑手接单方法
    {
        var order = activeOrders.Find(o => o.orderID == orderID);
        if (order != null && order.status == Order.OrderStatus.Pending)
        {
            order.status = Order.OrderStatus.InProgress;
            NotifyOrderUpdate(order);// 改为调用统一通知方法
        }
    }
    // 订单完成方法
    public void CompleteOrder(Order completedOrder)
    {
        if (activeOrders.Contains(completedOrder))
        {
            // 1. 从进行中列表移除
            activeOrders.Remove(completedOrder);

            // 2. 添加到已完成列表（可选）
            completedOrders.Add(completedOrder);

            // 3. 更新订单状态
            completedOrder.status = Order.OrderStatus.Completed;

            // 4. 触发相关事件（例如：UI更新、分数计算等）
            Debug.Log($"订单 {completedOrder.orderID} 已完成！");

            // 5. 可以在此处添加其他逻辑：
            // - 发放奖励
            // - 更新玩家分数
            // - 保存游戏进度
            // - 触发任务完成事件
        }
    }

    public void NotifyOrderUpdate(Order updatedOrder)
    {
        // 确保订单存在于管理列表中
        if (activeOrders.Contains(updatedOrder))
        {
            // 触发订单更新事件
            OnOrderChanged?.Invoke(updatedOrder);
        }
        else
        {
            Debug.LogWarning($"订单 {updatedOrder.orderID} 不在管理列表中");
        }
    }
}
