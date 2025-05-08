using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance { get; private set; }

    public delegate void OrderUpdateHandler(Order updatedOrder);
    public event OrderUpdateHandler OnOrderChanged;

    public List<Order> activeOrders = new List<Order>();
    public List<Order> GetAllOrders() => new List<Order>(activeOrders);//返回OrderManager类中的所有订单
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddOrder(Order newOrder)
    {
        activeOrders.Add(newOrder);
        OnOrderChanged?.Invoke(newOrder);
    }

    public void AcceptOrder(string orderId)
    {
        var order = activeOrders.Find(o => o.orderID == orderId);
        if (order != null && order.status == Order.OrderStatus.Pending)
        {
            order.status = Order.OrderStatus.InProgress;
            OnOrderChanged?.Invoke(order);
        }
    }
   

}
