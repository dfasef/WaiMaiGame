using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Merchant : MonoBehaviour
{
    [Header("����������������")]
    // ��Ӷ���״̬���ٺ�Э�̿���
    private Order MerNewOrder;//�¶���
    private float distanceToCustomer; //�˿;����̼ҵ���С����
    private Coroutine generateOrderCoroutine;//���ɶ�����Э��
    //�˿�λ���б�
    public List<Transform> customers = new List<Transform>();
    // ��Inspector������������ɶ���ʱ�䷶Χ
    public float minInterval = 3f;
    public float maxInterval = 5f;

    public GameObject messagePrefab;



    void Start()
    {
        messagePrefab.SetActive(false);
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
            if (MerNewOrder == null || MerNewOrder.status== Order.OrderStatus.Completed||
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
            orderID = ""+Random.Range(1000, 9999),
            merchantPosition = transform.position,
            CustomerPosition = customerPos,
            distance = Vector2.Distance(transform.position, customerPos),
            status = Order.OrderStatus.Pending
        };

        // ��ӵ�����ϵͳ
        OrderManager.Instance.AddOrder(MerNewOrder);

        Debug.Log("�����¶���"+MerNewOrder.orderID);

        messagePrefab.SetActive(true);
      
    }
   
   

    // ��������˿�λ��
    private Vector2 GenerateRandomCustomerPos()
    {
        if (customers.Count == 0) return Vector2.zero;
        int randomIndex = Random.Range(0, customers.Count);
        return customers[randomIndex].position;
    }
}
