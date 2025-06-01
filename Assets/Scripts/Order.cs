using UnityEngine;



public class Order : MonoBehaviour
{
    public string merchantID;//商家ID
    public string orderID;//订单ID
    public Vector2 merchantPosition;  // 商家坐标
    public Vector2 CustomerPosition;  // 送餐地址
    public float distance;  // 与商家距离
    public OrderStatus status;
   
    //TODO: add时间限制

    public enum OrderStatus
    {
        Pending,     // 待接单
        Accepted,    // 已接单
        InProgress,  // 配送中
        Completed    // 已完成
    }
}
