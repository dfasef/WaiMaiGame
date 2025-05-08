using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiderSelectPanel : BasePanel
{
    public void CloseRiderSelectPanel()
    {
        UIManager.Instance.ClosePanel(UIConst.RiderSelectPanel);
    }
}
