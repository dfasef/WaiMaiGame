using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public enum RiderState
{
    Rest,
    WaitingOrder,
    work
    //PickUpFood,
    //DeliverFood
}
public class deliveryMan : MonoBehaviour
{

    public RiderState riderState = RiderState.WaitingOrder;
    [Header("A星设置")]
    [SerializeField] private float wanderRadius = 10f; // 闲逛半径
    public IAstarAI ai; // A星接口
    private Vector2 initialPosition;
    

    [Header("订单管理")]
    public Order currentOrder;
    public System.Action<deliveryMan> OnStateChanged;
    public MerOrderManager currentMerOrderManager;
    [Header("骑手属性")]
    public int riderID;//骑手ID


    void Start()
    {
        // 获取A星组件
         ai = GetComponent<AIPath>();
        initialPosition = transform.position;
        SetRandomDestination();//随机生成闲逛目的地
    }
    

    private void Update()
    {
        switch (riderState)
        {
            case RiderState.Rest:
                RestUpdate();
                break;
            case RiderState.WaitingOrder:
                WaitingOrderUpdate();
                break;
            case RiderState.work:
                WorkUpdate();
                break;
        }
    }
    
    
    public void AssignOrder(Order order)//接单
    {
        currentOrder = order;
        currentOrder.status = Order.OrderStatus.Accepted;
        riderState = RiderState.work;
        StartCoroutine(ExecuteDelivery());
        // 通知订单变更
        MerOrderManager merOrderManager = OrderManager.Instance.GetMerOrderManager(order.merchantID);
        merOrderManager.NotifyOrderUpdate(currentOrder);
        OnStateChanged?.Invoke(this);
    }
   

    IEnumerator ExecuteDelivery()
    {
        // 步骤1：前往商家
        SetMercPositin(currentOrder.merchantPosition);
        yield return new WaitUntil(() => ai.reachedEndOfPath);

        // 步骤2：模拟取餐等待
        yield return new WaitForSeconds(2f);

        // 步骤3：前往送餐点
        SetMercPositin(currentOrder.CustomerPosition);
        yield return new WaitUntil(() => ai.reachedEndOfPath);

        // 在顾客位置停留3秒
        yield return new WaitForSeconds(5f);

        // 完成订单并转换状态
        currentOrder.status = Order.OrderStatus.Completed;
        MerOrderManager merOrderManager = OrderManager.Instance.GetMerOrderManager(currentOrder.merchantID);
        merOrderManager.CompleteOrder(currentOrder);
        TransitionToWaitingOrder();
       
    }
    public void SetMercPositin(Vector2 target)//设置商家位置
    {
        ai.destination = target;
        ai.SearchPath();
    }

    void WorkUpdate()
    {
        
    }
    void RestUpdate()
    {

    }
    void WaitingOrderUpdate()
    {
        
        // 持续检测是否到达当前目标点
        if (ai.reachedEndOfPath)
        {
            SetRandomDestination();
        }

    }
    void SetRandomDestination()
    {
        initialPosition = transform.position;
        // 随机生成目标点（类似扔飞盘）
        Vector2 randomPoint = initialPosition + Random.insideUnitCircle * wanderRadius;

        // 使用A星插件获取可行走点（确保不会卡墙里）
        GraphNode node = AstarPath.active.GetNearest(randomPoint, NNConstraint.Default).node;
        if (node != null)
        {
            Vector3 targetPosition = (Vector3)node.position;

            // 设置目的地（就像给狗狗指飞盘落点）
            ai.destination = targetPosition;
            ai.SearchPath();

        }
    }
   
    void TransitionToRest()
    {
        riderState = RiderState.Rest;
        OnStateChanged?.Invoke(this);
    }
    public void TransitionToWaitingOrder()
    {
        riderState = RiderState.WaitingOrder;
        OnStateChanged?.Invoke(this);
    }
    void TransitionToWork()
    {
        riderState = RiderState.work;
        OnStateChanged?.Invoke(this);
    }
}
