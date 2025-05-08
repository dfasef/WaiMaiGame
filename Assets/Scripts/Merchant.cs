using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Merchant : MonoBehaviour
{
    [Header("����������������")]
    // ��Ӷ���״̬���ٺ�Э�̿���
    private Order currentOrder;
    private Coroutine generateOrderCoroutine;
    // ��ǰ���еĶ����б�
    private List<Order> activeOrders = new List<Order>();
    //�˿�λ���б�
    public List<Transform> customers = new List<Transform>();
    // ��Inspector���������ʱ�䷶Χ
    public float minInterval = 3f;
    public float maxInterval = 5f;

    public GameObject Message;



    void Start()
    {
        Message.SetActive(false);
        // �����Զ����ɶ�����Э��
        generateOrderCoroutine = StartCoroutine(GenerateOrderRoutine());
    }

    void Update()
    {
        
    }

    // ��������Э��
    private IEnumerator GenerateOrderRoutine()
    {
        while (true)
        {
            // �ȴ����ʱ��
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));

            // ����û��δ������ʱ�����¶���
            if (currentOrder == null || currentOrder.status== Order.OrderStatus.Completed||
                currentOrder.status == Order.OrderStatus.InProgress)
            {
                CreateNewOrder();
            }
        }
    }
    
    public void CreateNewOrder()
    {
        // �����¶���
        currentOrder= new Order()
        {
            orderID = "ORD" + Random.Range(1000, 9999),
            merchantPosition = transform.position,
            CustomerPosition = GenerateRandomDeliveryPoint(),
            status = Order.OrderStatus.Pending
        };

        // ��ӵ�����ϵͳ
        OrderManager.Instance.AddOrder(currentOrder);

        Debug.Log("�����¶���"+currentOrder.orderID);

        Message.SetActive(true);
      
    }
   
   

    // ��������˿�λ��
    private Vector2 GenerateRandomDeliveryPoint()
    {
        if (customers.Count == 0) return Vector2.zero;
        int randomIndex = Random.Range(0, customers.Count);
        return customers[randomIndex].position;
    }
}
