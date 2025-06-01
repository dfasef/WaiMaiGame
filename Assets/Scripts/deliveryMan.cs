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
    [Header("A������")]
    [SerializeField] private float wanderRadius = 10f; // �й�뾶
    public IAstarAI ai; // A�ǽӿ�
    private Vector2 initialPosition;
    

    [Header("��������")]
    public Order currentOrder;
    public System.Action<deliveryMan> OnStateChanged;
    public MerOrderManager currentMerOrderManager;
    [Header("��������")]
    public int riderID;//����ID


    void Start()
    {
        // ��ȡA�����
         ai = GetComponent<AIPath>();
        initialPosition = transform.position;
        SetRandomDestination();//��������й�Ŀ�ĵ�
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
    
    
    public void AssignOrder(Order order)//�ӵ�
    {
        currentOrder = order;
        currentOrder.status = Order.OrderStatus.Accepted;
        riderState = RiderState.work;
        StartCoroutine(ExecuteDelivery());
        // ֪ͨ�������
        MerOrderManager merOrderManager = OrderManager.Instance.GetMerOrderManager(order.merchantID);
        merOrderManager.NotifyOrderUpdate(currentOrder);
        OnStateChanged?.Invoke(this);
    }
   

    IEnumerator ExecuteDelivery()
    {
        // ����1��ǰ���̼�
        SetMercPositin(currentOrder.merchantPosition);
        yield return new WaitUntil(() => ai.reachedEndOfPath);

        // ����2��ģ��ȡ�͵ȴ�
        yield return new WaitForSeconds(2f);

        // ����3��ǰ���Ͳ͵�
        SetMercPositin(currentOrder.CustomerPosition);
        yield return new WaitUntil(() => ai.reachedEndOfPath);

        // �ڹ˿�λ��ͣ��3��
        yield return new WaitForSeconds(5f);

        // ��ɶ�����ת��״̬
        currentOrder.status = Order.OrderStatus.Completed;
        MerOrderManager merOrderManager = OrderManager.Instance.GetMerOrderManager(currentOrder.merchantID);
        merOrderManager.CompleteOrder(currentOrder);
        TransitionToWaitingOrder();
       
    }
    public void SetMercPositin(Vector2 target)//�����̼�λ��
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
        
        // ��������Ƿ񵽴ﵱǰĿ���
        if (ai.reachedEndOfPath)
        {
            SetRandomDestination();
        }

    }
    void SetRandomDestination()
    {
        initialPosition = transform.position;
        // �������Ŀ��㣨�����ӷ��̣�
        Vector2 randomPoint = initialPosition + Random.insideUnitCircle * wanderRadius;

        // ʹ��A�ǲ����ȡ�����ߵ㣨ȷ�����Ῠǽ�
        GraphNode node = AstarPath.active.GetNearest(randomPoint, NNConstraint.Default).node;
        if (node != null)
        {
            Vector3 targetPosition = (Vector3)node.position;

            // ����Ŀ�ĵأ����������ָ������㣩
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
