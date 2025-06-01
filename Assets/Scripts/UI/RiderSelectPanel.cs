using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RiderSelectPanel : BasePanel
{
    public static RiderSelectPanel Instance { get; private set; }
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
    [Header("UI���")]
    [SerializeField] Text titleText;//����
    public Transform riderItemList;//����UI�б�����������ʹ��Vertical Layout Group��
    [Header("����UIԤ����")]
    public GameObject[] riderItemPrefabs; // ��ͬ�������Ͷ�Ӧ��ͬԤ����


    private Order targetOrder;

    public void Show(Order order)
    {
        targetOrder = order;
        titleText.text = $"Ϊ���� {order.orderID} ѡ������";
        RefreshRiderList();
    }

    void RefreshRiderList()
    {
        // ���������
        foreach (Transform child in riderItemList)
            Destroy(child.gameObject);

        // ��ȡ�������ֶ��ǽ���������
        var allRiders = RiderManager.Instance.GetAllRiders();

        foreach (var rider in allRiders)
        {
            // �����������ͻ�ȡԤ�������������
            int prefabIndex = rider.riderID ;
            GameObject targetPrefab = riderItemPrefabs[prefabIndex];

            // ʵ������ӦԤ����
            var item = Instantiate(targetPrefab, riderItemList);

            // ��ȡ�б������
            var listItem = item.GetComponent<RiderListItem>();
            // ��ʼ��ʱ���ݿ���״̬
            bool isAvailable = (rider.riderState == RiderState.WaitingOrder);
            listItem.Initialize(rider, OnRiderSelected, isAvailable);
        }
       
    }

    void OnRiderSelected(deliveryMan selectedRider)
    {
        if (selectedRider.riderState != RiderState.WaitingOrder)
        {
            Debug.LogWarning("�����ֵ�ǰ�޷��ӵ�");
            return;
        }

        // ���䶩��
        selectedRider.AssignOrder(targetOrder);
        //��ȡ�̼Ҷ���������
        MerOrderManager merchantOrderManager =
            OrderManager.Instance.GetMerOrderManager(targetOrder.merchantID);
        // ���¶���״̬
        targetOrder.status = Order.OrderStatus.InProgress;
        merchantOrderManager.NotifyOrderUpdate(targetOrder);
        // �ر�ѡ�����
        UIManager.Instance.ClosePanel(UIConst.RiderSelectPanel);
    }
  
}
