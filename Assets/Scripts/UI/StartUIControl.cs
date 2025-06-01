using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartUIControl : MonoBehaviour
{
    [Header("UI����")]
    public TextMeshProUGUI pressAnyKeyText; // ����UI Text�����������ʾ��ʾ����
    public float fadeSpeed = 1f; // ������˸�ٶ�

   

    void Start()
    {
        // ������ʾ����
        if (pressAnyKeyText != null)
            pressAnyKeyText.gameObject.SetActive(false);

        // �ӳ�1���������ʾ
        Invoke("EnablePrompt", 1f);
    }

    void EnablePrompt()// ������ʾ����
    {
        if (pressAnyKeyText != null)
        {
            pressAnyKeyText.gameObject.SetActive(true);
            StartCoroutine(TextFlicker());
        }
    }

    // ������˸Э��
    IEnumerator TextFlicker()
    {
        while (true)
        {
            float alpha = Mathf.PingPong(Time.time * fadeSpeed, 1);
            if (pressAnyKeyText != null)
            {
                pressAnyKeyText.color = new Color(pressAnyKeyText.color.r, pressAnyKeyText.color.g, pressAnyKeyText.color.b, alpha);
            }
            yield return null;
        }
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            ShowNextObject();
        }
    }

    // �л�����һ��GameObject
    void ShowNextObject()
    {
       
            Invoke("LoadNextScene", 1f);
    }
    void LoadNextScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
