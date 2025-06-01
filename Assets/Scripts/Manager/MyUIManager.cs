using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyUIManager : MonoBehaviour
{
    public static MyUIManager Instance;
    void Awake()
    {
        Instance = this;
    }
    public GameObject MerchantOrdPanel;
    public void OpenMerchantOrdPanel()
    {
        MerchantOrdPanel.SetActive(true);
    }
}
