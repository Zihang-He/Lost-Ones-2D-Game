// using UnityEngine;
// using UnityEngine.Events;
// using System.Collections.Generic;

// public class main : MonoBehaviour
// {
//     public ButtonHandler[] buttons;          // 三个按钮
//     public CameraTeleport cameraTeleport;    // 你的 CameraTeleport 脚本

//     private HashSet<string> clickedButtons = new HashSet<string>();

//     void Start()
//     {
//         // 给每个按钮绑定监听事件
//         foreach (var button in buttons)
//         {
//             button.OnButtonClicked.AddListener(OnAnyButtonClicked);
//         }
//     }

//     void OnAnyButtonClicked(string buttonName)
//     {
//         // 记录按钮点击
//         clickedButtons.Add(buttonName);
//         Debug.Log($"ButtonManager: {buttonName} clicked.");

//         // 如果三个按钮都点击过一次，触发 CameraTeleport
//         if (clickedButtons.Count >= buttons.Length)
//         {
//             Debug.Log("All buttons clicked! Triggering CameraTeleport.");
//             cameraTeleport.StartSceneTransition();  // 调用场景切换
//             clickedButtons.Clear();                 // 清空记录，支持循环点击
//         }
//     }
// }





using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Text.RegularExpressions; // 用于正则解析

public class main : MonoBehaviour
{
    [Header("按钮集合")]
    public ButtonHandler[] buttons;

    [Header("镜头脚本")]
    public CameraTeleport cameraTeleport;

    private HashSet<string> clickedButtons = new HashSet<string>();

    void Start()
    {
        // 绑定所有按钮的全局监听
        foreach (var button in buttons)
        {
            button.OnButtonClicked.AddListener(OnAnyButtonClicked);
        }
    }

    void OnAnyButtonClicked(string message)
    {
        Debug.Log($"[Main] 收到按钮事件：{message}");

        if (string.IsNullOrEmpty(message))
        {
            Debug.LogWarning("空的按钮名称，忽略。");
            return;
        }

        // 解析幕编号
        string actCode = ParseSceneCode(message);

        if (string.IsNullOrEmpty(actCode))
        {
            Debug.LogWarning($"未能识别幕编号（message={message}）");
            return;
        }

        // 根据幕编号执行不同逻辑
        switch (actCode)
        {
            case "S1":
                HandleAct1(message);
                break;

            case "S2":
                HandleAct2(message);
                break;

            case "S3":
                HandleAct3(message);
                break;

            case "S4":
                HandleAct4(message);
                break;

            case "S5":
                HandleAct5(message);
                break;

            default:
                Debug.LogWarning($"未识别的幕编号: {actCode}");
                break;
        }
    }

    // ✅ 用正则提取幕编号（如 S1、S2、S3...）
    string ParseSceneCode(string message)
    {
        Match match = Regex.Match(message, @"^(S\d+)");
        if (match.Success)
            return match.Groups[1].Value; // 返回 S1, S2, ...
        return null;
    }

    // ======================
    // 各幕逻辑分发函数
    // ======================

    void HandleAct1(string message)
    {
        Debug.Log($"[Act1] 收到事件：{message}");
        clickedButtons.Add(message);

        if (clickedButtons.Count >= buttons.Length)
        {
            Debug.Log("[Act1] 所有按钮已点击，触发转场");
            cameraTeleport.StartSceneTransition();
            clickedButtons.Clear();
        }
    }

    void HandleAct2(string message)
    {
        Debug.Log($"[Act2] 收到事件：{message}");
        // TODO: 第二幕逻辑
    }

    void HandleAct3(string message)
    {
        Debug.Log($"[Act3] 收到事件：{message}");
        // TODO: 第三幕逻辑
    }

    void HandleAct4(string message)
    {
        Debug.Log($"[Act4] 收到事件：{message}");
        // TODO: 第四幕逻辑
    }

    void HandleAct5(string message)
    {
        Debug.Log($"[Act5] 收到事件：{message}");
        // TODO: 第五幕逻辑
    }
}
