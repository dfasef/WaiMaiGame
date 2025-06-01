using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerOrderManager : MonoBehaviour
{
    [Header("订单生成数据设置")]
    // 添加订单状态跟踪和协程控制
    private Order MerNewOrder;//新订单
    public string merchantID; //商家ID
    private float distanceToCustomer; //顾客距离商家的最小距离
    private Coroutine generateOrderCoroutine;//生成订单的协程
    //顾客位置列表
    public List<Transform> customers = new List<Transform>();
    // 在Inspector中配置随机生成订单时间范围
    public float minInterval = 3f;
    public float maxInterval = 5f;
   
    public delegate void OrderUpdateHandler(Order updatedOrder);//订单更新事件
    public event OrderUpdateHandler OnOrderChanged;//订单更新事件
   
    public List<Order> activeOrders = new List<Order>();//进行中订单列表
    private List<Order> completedOrders = new List<Order>();// 已完成的订单列表

    public List<Order> GetAllOrders() => new List<Order>(activeOrders);//返回OrderManager类中的所有订单
    public GameObject messagePrefab;
  
    void Start()
    {
        RegisterWithOrderManager();// 注册到总订单管理器
        messagePrefab.SetActive(false);
        // 启动自动生成订单的协程
        generateOrderCoroutine = StartCoroutine(GenerateOrderRoutine());
    }
    // 订单生成协程
    private IEnumerator GenerateOrderRoutine()
    {
        while (true)
        {
            // 等待随机时间
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));

            // 仅当没有未处理订单时生成新订单
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
        // 生成新订单
        MerNewOrder = new Order()
        {
            orderID = "" + Random.Range(1000, 9999),
            merchantID=merchantID,
            merchantPosition = transform.position,
            CustomerPosition = customerPos,
            distance = Vector2.Distance(transform.position, customerPos),
            status = Order.OrderStatus.Pending
        };

        // 添加到订单系统
       this.AddOrder(MerNewOrder);

        Debug.Log("生成新订单" + MerNewOrder.orderID);

        messagePrefab.SetActive(true);

    }


    // 生成随机顾客位置
    private Vector2 GenerateRandomCustomerPos()
    {
        if (customers.Count == 0) return Vector2.zero;
        int randomIndex = Random.Range(0, customers.Count);
        return customers[randomIndex].position;
    }
    public void AddOrder(Order newOrder)// 新增订单方法
    {
        activeOrders.Add(newOrder);
        OnOrderChanged?.Invoke(newOrder);
    }

    public void AcceptOrder(string orderID)/// 被骑手接单方法
    {
        var order = activeOrders.Find(o => o.orderID == orderID);
        if (order != null && order.status == Order.OrderStatus.Pending)
        {
            order.status = Order.OrderStatus.InProgress;
            NotifyOrderUpdate(order);// 改为调用统一通知方法
        }
    }
    // 订单完成方法
    public void CompleteOrder(Order completedOrder)
    {
        if (activeOrders.Contains(completedOrder))
        {
            // 1. 从进行中列表移除
            activeOrders.Remove(completedOrder);

            // 2. 添加到已完成列表（可选）
            completedOrders.Add(completedOrder);

            // 3. 更新订单状态
            completedOrder.status = Order.OrderStatus.Completed;

            // 4. 触发相关事件（例如：UI更新、分数计算等）
            Debug.Log($"订单 {completedOrder.orderID} 已完成！");

            // 5. 可以在此处添加其他逻辑：
            // - 发放奖励
            // - 更新玩家分数
            // - 保存游戏进度
            // - 触发任务完成事件
        }
    }

    public void NotifyOrderUpdate(Order updatedOrder)
    {
        // 确保订单存在于管理列表中
        if (activeOrders.Contains(updatedOrder))
        {
            // 触发订单更新事件
            OnOrderChanged?.Invoke(updatedOrder);
        }
        else
        {
            Debug.LogWarning($"订单 {updatedOrder.orderID} 不在管理列表中");
        }
    }
    void RegisterWithOrderManager()// 注册到总订单管理器方法
    {
        if (OrderManager.Instance != null)
        {
            OrderManager.Instance.AddMerchantOrderManager(merchantID, this);
        }
        else
        {
            Debug.LogError("OrderManager 实例未找到");
        }
    }
}
