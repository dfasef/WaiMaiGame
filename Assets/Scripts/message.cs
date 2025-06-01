using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class message:MonoBehaviour
{
    public string merchentID;
    
    //µã»÷´ò¿ªui
    private void OnMouseDown()
    {
        MyUIManager.Instance.OpenMerchantOrdPanel();
        MerchantOrdPanel.Instance.ShowMerchantOrdPanel(merchentID);
       
    }

}
