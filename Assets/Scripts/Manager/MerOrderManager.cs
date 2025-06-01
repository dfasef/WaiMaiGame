using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerOrderManager : MonoBehaviour
{
    [Header("����������������")]
    // ��Ӷ���״̬���ٺ�Э�̿���
    private Order MerNewOrder;//�¶���
    public string merchantID; //�̼�ID
    private float distanceToCustomer; //�˿;����̼ҵ���С����
    private Coroutine generateOrderCoroutine;//���ɶ�����Э��
    //�˿�λ���б�
    public List<Transform> customers = new List<Transform>();
    // ��Inspector������������ɶ���ʱ�䷶Χ
    public float minInterval = 3f;
    public float maxInterval = 5f;
   
    public delegate void OrderUpdateHandler(Order updatedOrder);//���������¼�
    public event OrderUpdateHandler OnOrderChanged;//���������¼�
   
    public List<Order> activeOrders = new List<Order>();//�����ж����б�
    private List<Order> completedOrders = new List<Order>();// ����ɵĶ����б�

    public List<Order> GetAllOrders() => new List<Order>(activeOrders);//����OrderManager���е����ж���
    public GameObject messagePrefab;
  
    void Start()
    {
        RegisterWithOrderManager();// ע�ᵽ�ܶ���������
        messagePrefab.SetActive(false);
        // �����Զ����ɶ�����Э��
        generateOrderCoroutine = StartCoroutine(GenerateOrderRoutine());
    }
    // ��������Э��
    private IEnumerator GenerateOrderRoutine()
    {
        while (true)
        {
            // �ȴ����ʱ��
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));

            // ����û��δ������ʱ�����¶���
            if (MerNewOrder == null || MerNewOrder.status == Order.OrderStatus.Completed ||
                MerNewOrder.status == Order.OrderStatus.InProgress)
            {
                CreateNewOrder();
            }
        }
    }

    public void CreateNewOrder()
    {
        Vector2 customerPos = GenerateRandomCustomerPos();
        // �����¶���
        MerNewOrder = new Order()
        {
            orderID = "" + Random.Range(1000, 9999),
            merchantID=merchantID,
            merchantPosition = transform.position,
            CustomerPosition = customerPos,
            distance = Vector2.Distance(transform.position, customerPos),
            status = Order.OrderStatus.Pending
        };

        // ��ӵ�����ϵͳ
       this.AddOrder(MerNewOrder);

        Debug.Log("�����¶���" + MerNewOrder.orderID);

        messagePrefab.SetActive(true);

    }


    // ��������˿�λ��
    private Vector2 GenerateRandomCustomerPos()
    {
        if (customers.Count == 0) return Vector2.zero;
        int randomIndex = Random.Range(0, customers.Count);
        return customers[randomIndex].position;
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
    void RegisterWithOrderManager()// ע�ᵽ�ܶ�������������
    {
        if (OrderManager.Instance != null)
        {
            OrderManager.Instance.AddMerchantOrderManager(merchantID, this);
        }
        else
        {
            Debug.LogError("OrderManager ʵ��δ�ҵ�");
        }
    }
}
