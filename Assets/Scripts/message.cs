using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class message : BasePanel
{
    //�����ui
    private void OnMouseDown()
    {
        UIManager.Instance.OpenPanel(UIConst.MercOderPanel);
    }
}
