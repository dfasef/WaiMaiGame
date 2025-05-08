using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Order : MonoBehaviour
{
    public string orderID;
    public Vector2 merchantPosition;  // 商家坐标
    public Vector2 CustomerPosition;  // 送餐地址
    public OrderStatus status;
    //public Rider rider;//接单骑手
    //TODO: add时间限制

    public enum OrderStatus
    {
        Pending,     // 待接单
        Accepted,    // 已接单
        InProgress,  // 配送中
        Completed    // 已完成
    }
}
