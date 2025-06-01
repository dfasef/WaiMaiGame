using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    public class RiderListItem : MonoBehaviour
    {
    // UI�����
    public Text riderNameText;
    public Text statusText;
    public Button selectButton; // ������ť����

    private deliveryMan currentRider;
    private Action<deliveryMan> selectCallback;

    public void Initialize(deliveryMan riderData, Action<deliveryMan> clickCallback, bool isAvailable)
    {
        currentRider = riderData;
        selectCallback = clickCallback;

        // ��ʼ����ʾ
        riderNameText.text = riderData.gameObject.name;
        statusText.text = isAvailable ? "�ɽӵ�" : "������";
        selectButton.interactable = isAvailable;

        // �󶨰�ť�¼�
        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(OnSelectClick);
    }

    void OnSelectClick()
    {
        selectCallback?.Invoke(currentRider);
    }
}

