using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiderManager : MonoBehaviour
{
    public static RiderManager Instance { get; private set; }

    public List<deliveryMan> allRiders = new List<deliveryMan>();

    void Awake()
    {
        Instance = this;
        FindAllRiders();
    }

    void FindAllRiders()
    {
        allRiders.AddRange(FindObjectsOfType<deliveryMan>());
    }

    public List<deliveryMan> GetAllRiders()
    {

        return allRiders;

        //µÃµ½¿ÕÏÐÆïÊÖ
        //return allRiders.FindAll(r =>
        //    r.riderState == RiderState.WaitingOrder &&
        //    r.currentOrder == null
        //);
    }
}
