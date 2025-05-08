using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderUIItem : MonoBehaviour
{
    [SerializeField] Text orderIdText;
    [SerializeField] Text statusText;
    [SerializeField] Button acceptButton;

    public string OrderID { get; private set; }

    public void Initialize(Order order, UnityEngine.Events.UnityAction acceptCallback)
    {
        OrderID = order.orderID;
        orderIdText.text = $"订单号：{order.orderID}";
        UpdateStatus(order.status);

        acceptButton.onClick.RemoveAllListeners();
        acceptButton.onClick.AddListener(acceptCallback);
    }

    public void UpdateStatus(Order.OrderStatus status)
    {
        statusText.text = GetStatusText(status);
        acceptButton.interactable = (status == Order.OrderStatus.Pending);
    }

    string GetStatusText(Order.OrderStatus status)
    {
        return status switch
        {
            Order.OrderStatus.Pending => "等待接单",
            Order.OrderStatus.InProgress => "配送中",
            Order.OrderStatus.Completed => "已完成",
            _ => "未知状态"
        };
    }

    public void OpenRiderSelectPanel()
    {
        UIManager.Instance.OpenPanel(UIConst.RiderSelectPanel);
    }
   
}
