using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiderManager : MonoBehaviour
{
    public static RiderManager Instance { get; private set; }

    public List<Rider> allRiders = new List<Rider>();

    void Awake()
    {
        Instance = this;
        FindAllRiders();
    }

    void FindAllRiders()
    {
        allRiders.AddRange(FindObjectsOfType<Rider>());
    }

    public List<Rider> GetAllRiders()
    {

        return allRiders;


        //µÃµ½¿ÕÏÐÆïÊÖ
        //return allRiders.FindAll(r =>
        //    r.riderState == RiderState.WaitingOrder &&
        //    r.currentOrder == null
        //);
    }
}
