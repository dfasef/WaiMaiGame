using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OrderUIItem : MonoBehaviour
{
    [SerializeField] Text orderIdText;//������UI
    [SerializeField] Text statusText;//����״̬UI
    [SerializeField] Text distanceText;//���̼ҵľ���UI
    [SerializeField] Button assignButton;//�������ְ�ť

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
        distanceText.text = "����" + currentOrder.distance.ToString("F2") + "km";
        // ���ư�ť����״̬,�Ƿ�Ϊ���ȴ��ӵ���������ǣ��������û���������ť
        assignButton.interactable = currentOrder.status == Order.OrderStatus.Pending;
    }
    
    public void OnOpenRiderSelectPanel()//������ѡ��
    {
        UIManager.Instance.OpenPanel(UIConst.RiderSelectPanel);
        RiderSelectPanel.Instance.Show(currentOrder);
    }
    //״̬���·���
    public void UpdateOrderStatus(Order.OrderStatus status)
    {
        currentOrder.status = status;
        UpdateDisplay();
    }


    string GetStatusText(Order.OrderStatus status)//��ȡ����״̬�ı�
    {
        return status switch
        {
            Order.OrderStatus.Pending => "���ӵ�",
            Order.OrderStatus.InProgress => "������",
            Order.OrderStatus.Completed => "�����",
            _ => "δ֪״̬"
        };
    }

    
   
}
