using UnityEngine;



public class Order : MonoBehaviour
{
    public string merchantID;//�̼�ID
    public string orderID;//����ID
    public Vector2 merchantPosition;  // �̼�����
    public Vector2 CustomerPosition;  // �Ͳ͵�ַ
    public float distance;  // ���̼Ҿ���
    public OrderStatus status;
   
    //TODO: addʱ������

    public enum OrderStatus
    {
        Pending,     // ���ӵ�
        Accepted,    // �ѽӵ�
        InProgress,  // ������
        Completed    // �����
    }
}
