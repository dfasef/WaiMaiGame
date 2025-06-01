using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartUIControl : MonoBehaviour
{
    [Header("UI设置")]
    public TextMeshProUGUI pressAnyKeyText; // 拖入UI Text组件，用于显示提示文字
    public float fadeSpeed = 1f; // 文字闪烁速度

   

    void Start()
    {
        // 隐藏提示文字
        if (pressAnyKeyText != null)
            pressAnyKeyText.gameObject.SetActive(false);

        // 延迟1秒后启用提示
        Invoke("EnablePrompt", 1f);
    }

    void EnablePrompt()// 启用提示文字
    {
        if (pressAnyKeyText != null)
        {
            pressAnyKeyText.gameObject.SetActive(true);
            StartCoroutine(TextFlicker());
        }
    }

    // 文字闪烁协程
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

    // 切换到下一个GameObject
    void ShowNextObject()
    {
       
            Invoke("LoadNextScene", 1f);
    }
    void LoadNextScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
