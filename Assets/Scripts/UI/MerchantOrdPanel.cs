using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MerchantOrdPanel:BasePanel
{
    //�رն�������,���Ż�Ϊֱ�ӹر����
    public void CloseOrderPanel()
    {
        ClosePanel(UIConst.MercOderPanel);
    }

    [Header("UI����")]
    public Transform orderListParent; // ����UI�б�����������ʹ��Vertical Layout Group��
    public GameObject orderItemPrefab; // ������Ԥ�Ƽ�

    [Header("����")]
    public List<Order> visibleOrders = new List<Order>();// ��ʾ������ϵĶ����б�

    void OnEnable()
    {
        // ��UI����ʱˢ���б�
        RefreshOrderList();

        // ע�ᶩ�������¼�
        OrderManager.Instance.OnOrderChanged += HandleOrderUpdate;
    }

    void OnDisable()
    {
        // ע���¼�
        if (OrderManager.Instance != null)
            OrderManager.Instance.OnOrderChanged -= HandleOrderUpdate;
    }

    // ˢ�����������б�
    public void RefreshOrderList()
    {
        // �������UI
        foreach (Transform child in orderListParent)
            Destroy(child.gameObject);

        // ��ȡ���л�Ծ����
        var orders = OrderManager.Instance.GetAllOrders();

        // �����µ�UI��
        foreach (var order in orders)
        {
            CreateOrderUIItem(order);
        }
    }

    // ������������UI��
    void CreateOrderUIItem(Order order)
    {
       
        GameObject item = Instantiate(orderItemPrefab, orderListParent);
        OrderUIItem uiItem = item.GetComponent<OrderUIItem>();
        uiItem.Initialize(order);
    }

    // ���������¼�����
    void HandleOrderUpdate(Order updatedOrder)
    {
        RefreshOrderList();
        // ���Ҷ�ӦUI�����״̬
        foreach (Transform child in orderListParent)
        {
            OrderUIItem uiItem = child.GetComponent<OrderUIItem>();
            if (uiItem.OrderID == updatedOrder.orderID)
            {
                uiItem.UpdateOrderStatus(updatedOrder.status);//���¸�UI��״̬
                break;
            }
        }
    }
}


