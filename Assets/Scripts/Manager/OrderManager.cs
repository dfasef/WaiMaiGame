using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    

    public static OrderManager Instance { get; private set; }

    public delegate void OrderUpdateHandler(Order updatedOrder);
    public event OrderUpdateHandler OnOrderChanged;
   
    public List<Order> activeOrders = new List<Order>();//�����ж����б�
    
    private List<Order> completedOrders = new List<Order>();// ����ɵĶ����б�
    public List<Order> GetAllOrders() => new List<Order>(activeOrders);//����OrderManager���е����ж���
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddOrder(Order newOrder)// ������������
    {
        activeOrders.Add(newOrder);
        OnOrderChanged?.Invoke(newOrder);
    }

    public void AcceptOrder(string orderID)/// �����ֽӵ�����
    {
        var order = activeOrders.Find(o => o.orderID == orderID);
        if (order != null && order.status == Order.OrderStatus.Pending)
        {
            order.status = Order.OrderStatus.InProgress;
            NotifyOrderUpdate(order);// ��Ϊ����ͳһ֪ͨ����
        }
    }
    // ������ɷ���
    public void CompleteOrder(Order completedOrder)
    {
        if (activeOrders.Contains(completedOrder))
        {
            // 1. �ӽ������б��Ƴ�
            activeOrders.Remove(completedOrder);

            // 2. ��ӵ�������б���ѡ��
            completedOrders.Add(completedOrder);

            // 3. ���¶���״̬
            completedOrder.status = Order.OrderStatus.Completed;

            // 4. ��������¼������磺UI���¡���������ȣ�
            Debug.Log($"���� {completedOrder.orderID} ����ɣ�");

            // 5. �����ڴ˴���������߼���
            // - ���Ž���
            // - ������ҷ���
            // - ������Ϸ����
            // - ������������¼�
        }
    }

    public void NotifyOrderUpdate(Order updatedOrder)
    {
        // ȷ�����������ڹ����б���
        if (activeOrders.Contains(updatedOrder))
        {
            // �������������¼�
            OnOrderChanged?.Invoke(updatedOrder);
        }
        else
        {
            Debug.LogWarning($"���� {updatedOrder.orderID} ���ڹ����б���");
        }
    }
}
