using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Merchant : MonoBehaviour
{
    [Header("订单生成数据设置")]
    // 添加订单状态跟踪和协程控制
    private Order MerNewOrder;//新订单
    private float distanceToCustomer; //顾客距离商家的最小距离
    private Coroutine generateOrderCoroutine;//生成订单的协程
    //顾客位置列表
    public List<Transform> customers = new List<Transform>();
    // 在Inspector中配置随机生成订单时间范围
    public float minInterval = 3f;
    public float maxInterval = 5f;

    public GameObject messagePrefab;



    void Start()
    {
        messagePrefab.SetActive(false);
        // 启动自动生成订单的协程
        generateOrderCoroutine = StartCoroutine(GenerateOrderRoutine());
    }

    void Update()
    {
        
    }

    // 订单生成协程
    private IEnumerator GenerateOrderRoutine()
    {
        while (true)
        {
            // 等待随机时间
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));

            // 仅当没有未处理订单时生成新订单
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
        // 生成新订单
        MerNewOrder = new Order()
        {
            orderID = ""+Random.Range(1000, 9999),
            merchantPosition = transform.position,
            CustomerPosition = customerPos,
            distance = Vector2.Distance(transform.position, customerPos),
            status = Order.OrderStatus.Pending
        };

        // 添加到订单系统
        OrderManager.Instance.AddOrder(MerNewOrder);

        Debug.Log("生成新订单"+MerNewOrder.orderID);

        messagePrefab.SetActive(true);
      
    }
   
   

    // 生成随机顾客位置
    private Vector2 GenerateRandomCustomerPos()
    {
        if (customers.Count == 0) return Vector2.zero;
        int randomIndex = Random.Range(0, customers.Count);
        return customers[randomIndex].position;
    }
}
