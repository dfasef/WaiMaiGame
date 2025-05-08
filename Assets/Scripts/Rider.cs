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
public class Rider : MonoBehaviour
{

    public RiderState riderState = RiderState.WaitingOrder;
    [Header("A������")]
    [SerializeField] private float wanderRadius = 10f; // �й�뾶
    public IAstarAI ai; // A�ǽӿ�
    private Vector2 initialPosition;
    private Seeker seeker;

    [Header("��������")]
    private Order currentOrder;
   

    void Start()
    {
        // ��ȡA�����
         ai = GetComponent<AIPath>();
        initialPosition = transform.position;
        SetRandomDestination();
       

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
        //OrderManager.Instance.CompleteOrder(currentOrder);
        TransitionToWaitingOrder();
       
    }
    public void SetMercPositin(Vector2 target)
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
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    AssignOrder(OrderManager.Instance.activeOrders[0]);
        //    TransitionToWork();
        //}
           
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
    void PickUpFoodUpdate()
    {

    }
    void DeliverFoodUpdate()
    {

    }


    //protected virtual void EnableUpdate()
    //{

    //}
    void TransitionToRest()
    {
        riderState = RiderState.Rest;
        GetComponent<Animator>().enabled = false;

        GetComponent<Collider2D>().enabled = false;
    }
    public void TransitionToWaitingOrder()
    {
        riderState = RiderState.WaitingOrder;
        GetComponent<Animator>().enabled = true;

        GetComponent<Collider2D>().enabled = true;
    }
    void TransitionToWork()
    {
        riderState = RiderState.work;
    }
}
