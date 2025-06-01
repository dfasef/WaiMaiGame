using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private Dictionary<string, MerOrderManager> merchantDict = new Dictionary<string, MerOrderManager>();

    //�����̼�ע�ᶩ��������
    public void AddMerchantOrderManager(string merchantID, MerOrderManager manager)
    {
        if (!merchantDict.ContainsKey(merchantID))
        {
            merchantDict[merchantID] = manager;
        }
        else
        {
            Debug.LogError($"�̼� {merchantID} �Ѿ�ע���� MerOrderManager");
        }
    }


    //ͨ���̼�Id��ȡ��Ӧ�Ķ���������
    public MerOrderManager GetMerOrderManager(string merchantID)
    {
        if (merchantDict.TryGetValue(merchantID, out MerOrderManager manager))
        {
            return manager;
        }
        Debug.LogError($"�Ҳ����̼�{merchantID}��Ӧ��MerOrderManager");
        return null;
    }

}
