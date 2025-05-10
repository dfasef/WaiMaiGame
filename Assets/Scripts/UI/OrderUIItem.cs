using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OrderUIItem : MonoBehaviour
{
    [SerializeField] Text orderIdText;//订单号UI
    [SerializeField] Text statusText;//订单状态UI
    [SerializeField] Text distanceText;//与商家的距离UI
    [SerializeField] Button assignButton;//分配骑手按钮

    private Order currentOrder;
    private float distance;

    public string OrderID { get; private set; }

    public void Initialize(Order order)
    {
        
        currentOrder = order;
        OrderID = order.orderID;
        distance=order.distance;
        UpdateDisplay();
        assignButton.onClick.RemoveAllListeners();
        assignButton.onClick.AddListener(OnOpenRiderSelectPanel);
    }
    void UpdateDisplay()
    {
        orderIdText.text = "ID:"+currentOrder.orderID;
        statusText.text = GetStatusText(currentOrder.status);
        distanceText.text = "距离" + currentOrder.distance.ToString("F2") + "km";
        // 控制按钮交互状态,是否为“等待接单”，如果是，则允许用户点击这个按钮
        assignButton.interactable = currentOrder.status == Order.OrderStatus.Pending;
    }
    
    public void OnOpenRiderSelectPanel()//打开骑手选择
    {
        UIManager.Instance.OpenPanel(UIConst.RiderSelectPanel);
        RiderSelectPanel.Instance.Show(currentOrder);
    }
    //状态更新方法
    public void UpdateOrderStatus(Order.OrderStatus status)
    {
        currentOrder.status = status;
        UpdateDisplay();
    }


    string GetStatusText(Order.OrderStatus status)//获取订单状态文本
    {
        return status switch
        {
            Order.OrderStatus.Pending => "待接单",
            Order.OrderStatus.InProgress => "配送中",
            Order.OrderStatus.Completed => "已完成",
            _ => "未知状态"
        };
    }

    
   
}
