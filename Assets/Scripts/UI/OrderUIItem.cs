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
        orderIdText.text = $"�����ţ�{order.orderID}";
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
            Order.OrderStatus.Pending => "�ȴ��ӵ�",
            Order.OrderStatus.InProgress => "������",
            Order.OrderStatus.Completed => "�����",
            _ => "δ֪״̬"
        };
    }

    public void OpenRiderSelectPanel()
    {
        UIManager.Instance.OpenPanel(UIConst.RiderSelectPanel);
    }
   
}
