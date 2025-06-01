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
    [Header("UI组件")]
    [SerializeField] Text titleText;//标题
    public Transform riderItemList;//骑手UI列表容器（建议使用Vertical Layout Group）
    [Header("骑手UI预制体")]
    public GameObject[] riderItemPrefabs; // 不同骑手类型对应不同预制体


    private Order targetOrder;

    public void Show(Order order)
    {
        targetOrder = order;
        titleText.text = $"为订单 {order.orderID} 选择骑手";
        RefreshRiderList();
    }

    void RefreshRiderList()
    {
        // 清空现有项
        foreach (Transform child in riderItemList)
            Destroy(child.gameObject);

        // 获取所有骑手而非仅可用骑手
        var allRiders = RiderManager.Instance.GetAllRiders();

        foreach (var rider in allRiders)
        {
            // 根据骑手类型获取预制体索引或键名
            int prefabIndex = rider.riderID ;
            GameObject targetPrefab = riderItemPrefabs[prefabIndex];

            // 实例化对应预制体
            var item = Instantiate(targetPrefab, riderItemList);

            // 获取列表项组件
            var listItem = item.GetComponent<RiderListItem>();
            // 初始化时传递可用状态
            bool isAvailable = (rider.riderState == RiderState.WaitingOrder);
            listItem.Initialize(rider, OnRiderSelected, isAvailable);
        }
       
    }

    void OnRiderSelected(deliveryMan selectedRider)
    {
        if (selectedRider.riderState != RiderState.WaitingOrder)
        {
            Debug.LogWarning("该骑手当前无法接单");
            return;
        }

        // 分配订单
        selectedRider.AssignOrder(targetOrder);
        //获取商家订单管理器
        MerOrderManager merchantOrderManager =
            OrderManager.Instance.GetMerOrderManager(targetOrder.merchantID);
        // 更新订单状态
        targetOrder.status = Order.OrderStatus.InProgress;
        merchantOrderManager.NotifyOrderUpdate(targetOrder);
        // 关闭选择面板
        UIManager.Instance.ClosePanel(UIConst.RiderSelectPanel);
    }
  
}
