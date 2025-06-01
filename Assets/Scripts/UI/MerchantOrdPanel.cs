using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;


public class MerchantOrdPanel:MonoBehaviour
{
    //�رն�������,���Ż�Ϊֱ�ӹر����
    public void CloseOrderPanel()
    {
        this.gameObject.SetActive(false);
    }

    [Header("UI����")]
    public Transform orderListParent; // ����UI�б�����������ʹ��Vertical Layout Group��
    public GameObject orderItemPrefab; // ������Ԥ�Ƽ�

    [Header("����")]
    public List<Order> visibleOrders = new List<Order>();// ��ʾ������ϵĶ����б�
    public string currentMerchantID; // ��ǰ�̼�ID
 
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
        // ��UI����ʱˢ���б�
        RefreshOrderList();
        // ע�ᶩ�������¼�
        GetMerchantManager().OnOrderChanged += HandleOrderUpdate;
    }

    
    MerOrderManager GetMerchantManager()
    {
        // ��ȡ�̼ҹ�����
        MerOrderManager merchantOrderManager = OrderManager.Instance.
            GetMerOrderManager(currentMerchantID);
        return merchantOrderManager;
    }
    void OnDisable()
    {
        // ע���¼�
        if (GetMerchantManager() != null)
            GetMerchantManager().OnOrderChanged -= HandleOrderUpdate;
    }

    // ˢ�����������б�
    public void RefreshOrderList()
    {
        // �������UI
        foreach (Transform child in orderListParent)
            Destroy(child.gameObject);
        // ��ȡ���л�Ծ����
        var orders = GetMerchantManager().GetAllOrders();

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


