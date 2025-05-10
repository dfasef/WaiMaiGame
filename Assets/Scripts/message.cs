using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class message : BasePanel
{
    //µã»÷´ò¿ªui
    private void OnMouseDown()
    {
        UIManager.Instance.OpenPanel(UIConst.MercOderPanel);
    }
}
