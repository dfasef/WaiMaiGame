using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Merchant : MonoBehaviour
{
    [Header("订单生成数据设置")]
    // 添加订单状态跟踪和协程控制
    private Order currentOrder;
    private Coroutine generateOrderCoroutine;
    // 当前持有的订单列表
    private List<Order> activeOrders = new List<Order>();
    //顾客位置列表
    public List<Transform> customers = new List<Transform>();
    // 在Inspector中配置随机时间范围
    public float minInterval = 3f;
    public float maxInterval = 5f;

    public GameObject Message;



    void Start()
    {
        Message.SetActive(false);
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
            if (currentOrder == null || currentOrder.status== Order.OrderStatus.Completed||
                currentOrder.status == Order.OrderStatus.InProgress)
            {
                CreateNewOrder();
            }
        }
    }
    
    public void CreateNewOrder()
    {
        // 生成新订单
        currentOrder= new Order()
        {
            orderID = "ORD" + Random.Range(1000, 9999),
            merchantPosition = transform.position,
            CustomerPosition = GenerateRandomDeliveryPoint(),
            status = Order.OrderStatus.Pending
        };

        // 添加到订单系统
        OrderManager.Instance.AddOrder(currentOrder);

        Debug.Log("生成新订单"+currentOrder.orderID);

        Message.SetActive(true);
      
    }
   
   

    // 生成随机顾客位置
    private Vector2 GenerateRandomDeliveryPoint()
    {
        if (customers.Count == 0) return Vector2.zero;
        int randomIndex = Random.Range(0, customers.Count);
        return customers[randomIndex].position;
    }
}
