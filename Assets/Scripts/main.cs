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
	[System.Serializable]
	public class SceneRequirement
	{
		public string sceneCode = "S1"; // 形如 S1/S2/S3
		public int requiredButtons = 0; // 该幕需要点击的按钮总数
		public int requiredDrops = 0;   // 该幕需要正确放置的物品总数
	}

	[Header("按钮集合")]
	public ButtonHandler[] buttons;

	[Header("可拖拽物集合")]
	public DraggableItem[] draggableItems;

	[Header("各幕过关条件（按钮数 & 拖拽物数）")]
	public SceneRequirement[] sceneRequirements;

	[Header("镜头脚本")]
	public CameraTeleport cameraTeleport;

	// 每幕已完成的按钮与拖拽统计
	private readonly Dictionary<string, HashSet<string>> clickedButtonsByScene = new Dictionary<string, HashSet<string>>();
	private readonly Dictionary<string, HashSet<string>> droppedItemsByScene = new Dictionary<string, HashSet<string>>();
	private readonly HashSet<string> sceneAlreadyCompleted = new HashSet<string>();

	private Dictionary<string, SceneRequirement> requirementByScene;

	void Start()
	{
		// 建立查找表
		requirementByScene = new Dictionary<string, SceneRequirement>();
		foreach (var r in sceneRequirements)
		{
			if (!string.IsNullOrEmpty(r.sceneCode) && !requirementByScene.ContainsKey(r.sceneCode))
			{
				requirementByScene.Add(r.sceneCode, r);
			}
		}

		// 绑定按钮事件
		if (buttons != null)
		{
			foreach (var button in buttons)
			{
				button.OnButtonClicked.AddListener(OnAnyButtonClicked);
			}
		}

		// 绑定拖拽完成事件（带上对应物品的 sceneIndex）
		if (draggableItems != null)
		{
			foreach (var item in draggableItems)
			{
				var capturedItem = item; // 捕获当前引用
				capturedItem.OnItemDropped.AddListener((itemName) => OnAnyItemDropped(capturedItem.sceneIndex, itemName));
			}
		}
	}

	// 按钮点击统一入口（buttonName 推荐形如：S1_ButtonA 或 S2:Lever01）
	void OnAnyButtonClicked(string message)
	{
		if (string.IsNullOrEmpty(message)) return;
		string sceneCode = ParseSceneCode(message);
		if (string.IsNullOrEmpty(sceneCode)) return;

		if (!clickedButtonsByScene.ContainsKey(sceneCode))
		{
			clickedButtonsByScene[sceneCode] = new HashSet<string>();
		}
		clickedButtonsByScene[sceneCode].Add(message);

		TryCompleteScene(sceneCode);
	}

	// 拖拽物放置统一入口
	void OnAnyItemDropped(int sceneIndex, string itemName)
	{
		string sceneCode = IndexToSceneCode(sceneIndex);
		if (string.IsNullOrEmpty(sceneCode)) return;
		if (string.IsNullOrEmpty(itemName)) return;

		if (!droppedItemsByScene.ContainsKey(sceneCode))
		{
			droppedItemsByScene[sceneCode] = new HashSet<string>();
		}
		droppedItemsByScene[sceneCode].Add(itemName);

		TryCompleteScene(sceneCode);
	}

	// 判断该幕是否达成要求，如达成则触发转场
	void TryCompleteScene(string sceneCode)
	{
		if (sceneAlreadyCompleted.Contains(sceneCode)) return;
		if (requirementByScene == null || !requirementByScene.ContainsKey(sceneCode)) return;

		var req = requirementByScene[sceneCode];
		int doneButtons = clickedButtonsByScene.ContainsKey(sceneCode) ? clickedButtonsByScene[sceneCode].Count : 0;
		int doneDrops = droppedItemsByScene.ContainsKey(sceneCode) ? droppedItemsByScene[sceneCode].Count : 0;

		bool buttonsOk = req.requiredButtons <= 0 || doneButtons >= req.requiredButtons;
		bool dropsOk = req.requiredDrops <= 0 || doneDrops >= req.requiredDrops;

		if (buttonsOk && dropsOk)
		{
			sceneAlreadyCompleted.Add(sceneCode);
			Debug.Log($"[{sceneCode}] 所有条件已满足（按钮 {doneButtons}/{req.requiredButtons}，物品 {doneDrops}/{req.requiredDrops}），触发转场。");
			if (cameraTeleport != null)
			{
                Debug.Log("触发转场");
				cameraTeleport.StartSceneTransition();
			}
		}
	}

	// 提取消息中的幕编号：支持 "S1_xxx"、"S2:xxx"、"S3 xxx"
	string ParseSceneCode(string message)
	{
		Match match = Regex.Match(message, @"^(S\d+)");
		if (match.Success) return match.Groups[1].Value;
		match = Regex.Match(message, @"(S\d+)");
		return match.Success ? match.Groups[1].Value : null;
	}

	string IndexToSceneCode(int sceneCodeInt)
	{
		// 现在将传入的整数直接视为场景编号（1..N），不再做 +1 偏移
		return $"S{sceneCodeInt+1}";
	}
}
