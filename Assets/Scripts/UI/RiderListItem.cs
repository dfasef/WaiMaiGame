using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    public class RiderListItem : MonoBehaviour
    {
    // UI组件绑定
    public Text riderNameText;
    public Text statusText;
    public Button selectButton; // 新增按钮引用

    private Rider currentRider;
    private Action<Rider> selectCallback;

    public void Initialize(Rider riderData, Action<Rider> clickCallback, bool isAvailable)
    {
        currentRider = riderData;
        selectCallback = clickCallback;

        // 初始化显示
        riderNameText.text = riderData.gameObject.name;
        statusText.text = isAvailable ? "可接单" : "工作中";
        selectButton.interactable = isAvailable;

        // 绑定按钮事件
        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(OnSelectClick);
    }

    void OnSelectClick()
    {
        selectCallback?.Invoke(currentRider);
    }
}

