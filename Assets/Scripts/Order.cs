using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Order : MonoBehaviour
{
    public string orderID;
    public Vector2 merchantPosition;  // �̼�����
    public Vector2 CustomerPosition;  // �Ͳ͵�ַ
    public OrderStatus status;
    //public Rider rider;//�ӵ�����
    //TODO: addʱ������

    public enum OrderStatus
    {
        Pending,     // ���ӵ�
        Accepted,    // �ѽӵ�
        InProgress,  // ������
        Completed    // �����
    }
}
