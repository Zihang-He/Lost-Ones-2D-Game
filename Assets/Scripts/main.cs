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
        public int requiredMoves = 0;        // 该幕需要完成移动的对象总数
	}

	[Header("按钮集合")]
	public ButtonHandler[] buttons;

	[Header("可拖拽物集合")]
	public DraggableItem[] draggableItems;

	[Header("各幕过关条件（按钮数 & 拖拽物数）")]
	public SceneRequirement[] sceneRequirements;

	[Header("镜头脚本")]
	public CameraTeleport cameraTeleport;

	[Header("屏幕滤镜")]
	// public ScreenFilterController screenFilter;
	public Color filterOnEventColor = new Color(0f, 0f, 0f, 0.35f);

	[Header("灰度特效（相机上）")]
	public ScreenGrayscaleEffect grayscaleEffect;
	[Range(0f,1f)] public float grayscaleDesaturationOn = 1.0f;
	[Range(0.5f,2f)] public float grayscaleContrastOn = 1.0f;
	public float filterFadeDuration = 0.15f;

	[Header("高对比特效（相机上）")]
	public ScreenHighContrastEffect highContrastEffect;
	[Range(0.5f,3f)] public float highContrastOn = 1.6f;
	[Range(-1f,1f)] public float highContrastBrightnessOn = 0f;

    [Header("可移动对象集合")]
    public MoveObject[] movableObjects;

    

	// 每幕已完成的按钮与拖拽统计
    private readonly Dictionary<string, HashSet<string>> clickedButtonsByScene = new Dictionary<string, HashSet<string>>();
	private readonly Dictionary<string, HashSet<string>> droppedItemsByScene = new Dictionary<string, HashSet<string>>();
	private readonly HashSet<string> sceneAlreadyCompleted = new HashSet<string>();
    private readonly Dictionary<string, HashSet<string>> movedObjectsByScene = new Dictionary<string, HashSet<string>>();

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
        if (movableObjects != null)
    {
        foreach (var moveObj in movableObjects)
        {
            moveObj.OnMoveFinished += () => OnMoveObjectFinished(moveObj);
        }
    }

		// Ensure visual effects and music start in neutral state
		DisableHighContrast();
		DisableGrayscale();
		// if (BackgroundMusic.Instance != null)
		// {
		// 	BackgroundMusic.Instance.SwitchToNormal();
		// }
	}

	// 按钮点击统一入口（buttonName 推荐形如：S1_ButtonA 或 S2:Lever01）
	void OnAnyButtonClicked(string message)
	{
		if (string.IsNullOrEmpty(message)) return;
		string sceneCode = ParseSceneCode(message);
		if (string.IsNullOrEmpty(sceneCode)) return;
        Debug.Log("button triggered: " + message);

        if (message == "S3Button3"){
            if (BackgroundMusic.Instance != null)
                {
                    BackgroundMusic.Instance.SwitchToNormal();
                }
        }
        
		// 事件视觉反馈：按钮点击
		if (ShouldEnableGrayscale(message)) EnableGrayscale();
		if (ShouldDisableGrayscale(message)) DisableGrayscale();
		if (ShouldEnableHighContrast(message)) EnableHighContrast();
		if (ShouldDisableHighContrast(message)) DisableHighContrast();
		// // if (screenFilter != null && ShouldEnableOverlay(message)) screenFilter.ApplyOverlay(filterOnEventColor, filterFadeDuration);

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

		// 事件视觉反馈：物品放置
        Debug.Log("itemName: " + itemName);
		if (ShouldEnableGrayscale(itemName)) EnableGrayscale();
		if (ShouldDisableGrayscale(itemName)) DisableGrayscale();
		if (ShouldEnableHighContrast(itemName)) EnableHighContrast();
		if (ShouldDisableHighContrast(itemName)) DisableHighContrast();
		// // if (screenFilter != null && ShouldEnableOverlay(itemName)) screenFilter.ApplyOverlay(filterOnEventColor, filterFadeDuration);

		if (!droppedItemsByScene.ContainsKey(sceneCode))
		{
			droppedItemsByScene[sceneCode] = new HashSet<string>();
		}
		droppedItemsByScene[sceneCode].Add(itemName);

		TryCompleteScene(sceneCode);
	}
    void OnMoveObjectFinished(MoveObject moveObj)
    {
        if (moveObj == null) return;

        string message = moveObj.gameObject.name; // 或 MoveObject 内可加自定义名字字段
        string sceneCode = ParseSceneCode(message);
        if (string.IsNullOrEmpty(sceneCode)) return;

        if (!movedObjectsByScene.ContainsKey(sceneCode))
        {
            movedObjectsByScene[sceneCode] = new HashSet<string>();
        }
        movedObjectsByScene[sceneCode].Add(message);

        Debug.Log($"[{sceneCode}] MoveObject {message} 完成移动");

        // 尝试判定幕完成
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
        int doneMoves = movedObjectsByScene.ContainsKey(sceneCode) ? movedObjectsByScene[sceneCode].Count : 0;

        bool buttonsOk = req.requiredButtons <= 0 || doneButtons >= req.requiredButtons;
        bool dropsOk = req.requiredDrops <= 0 || doneDrops >= req.requiredDrops;
        bool movesOk = req.requiredMoves <= 0 || doneMoves >= req.requiredMoves;

		if (buttonsOk && dropsOk && movesOk)
		{
			sceneAlreadyCompleted.Add(sceneCode);
			Debug.Log($"[{sceneCode}] 所有条件已满足（按钮 {doneButtons}/{req.requiredButtons}，物品 {doneDrops}/{req.requiredDrops}），触发转场。");
			if (cameraTeleport != null)
			{
				Debug.Log("触发转场");
				// 场景切换时关闭特效与覆盖
				DisableGrayscale();
				DisableHighContrast();
				// if (screenFilter != null) screenFilter.ClearOverlay(filterFadeDuration);
				cameraTeleport.StartSceneTransition();
			}
		}
	}

	bool ShouldEnableGrayscale(string eventId)
	{
		// 根据需求开启灰度：匹配 S1Item2 或 S2Button3 等
        Debug.Log("eventId: " + eventId + ' ' + eventId == "S1Item2");
		return eventId == "S3Button2";
	}

	bool ShouldDisableGrayscale(string eventId)
	{
		// 当 S2Button1 触发或其他自定义事件则关闭
		return eventId == "S3Button3";
	}

// 	bool ShouldEnableOverlay(string eventId)
// 	{
// 		// 如需与灰度一致的覆盖触发，沿用同一规则；可按需扩展
// 		return ShouldEnableGrayscale(eventId);
// 	}

	void EnableGrayscale()
	{
        DisableHighContrast();
		if (grayscaleEffect != null)
		{
			grayscaleEffect.SetEnabled(true, grayscaleDesaturationOn, grayscaleContrastOn);
		}
	}

	void DisableGrayscale()
	{
		if (grayscaleEffect != null)
		{
			grayscaleEffect.SetEnabled(false, 0f);
		}
	}

	bool ShouldEnableHighContrast(string eventId)
	{
		// 示例：为特定按钮/物品开启高对比
		return eventId == "S3Button1";
	}

	bool ShouldDisableHighContrast(string eventId)
	{
		// 示例：某事件关闭高对比
		return eventId == "S3Button3";
	}

	void EnableHighContrast()
	{
        DisableGrayscale();
		if (highContrastEffect != null)
		{
			highContrastEffect.SetEnabled(true, highContrastOn, highContrastBrightnessOn);
		}
		// Switch to mania music when high contrast is enabled
		if (BackgroundMusic.Instance != null)
		{
			BackgroundMusic.Instance.SwitchToMania();
		}
	}

	void DisableHighContrast()
	{
		if (highContrastEffect != null)
		{
			highContrastEffect.SetEnabled(false, 1f, 0f);
		}
		// Switch back to normal music when high contrast is disabled
		// if (BackgroundMusic.Instance != null)
		// {
		// 	BackgroundMusic.Instance.SwitchToNormal();
		// }
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
