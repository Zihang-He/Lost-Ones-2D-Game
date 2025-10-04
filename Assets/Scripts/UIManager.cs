// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;
// using System.Collections;

// public class UIManager : MonoBehaviour
// {
//     public static UIManager Instance { get; private set; }
    
//     [Header("UI Panels")]
//     public GameObject inventoryPanel;
//     public GameObject dialoguePanel;
//     public GameObject puzzlePanel;
//     public GameObject tooltipPanel;
//     public GameObject itemInfoPanel;
    
//     [Header("Inventory UI")]
//     public Button inventoryToggleButton;
//     public TextMeshProUGUI inventoryTitle;
    
//     [Header("Dialogue UI")]
//     public TextMeshProUGUI dialogueText;
//     public Button[] dialogueButtons;
//     public TextMeshProUGUI[] dialogueButtonTexts;
    
//     [Header("Puzzle UI")]
//     public TextMeshProUGUI puzzleTitle;
//     public TextMeshProUGUI puzzleDescription;
//     public Button puzzleCloseButton;
    
//     [Header("Tooltip UI")]
//     public TextMeshProUGUI tooltipTitle;
//     public TextMeshProUGUI tooltipDescription;
    
//     [Header("Item Info UI")]
//     public Image itemInfoIcon;
//     public TextMeshProUGUI itemInfoName;
//     public TextMeshProUGUI itemInfoDescription;
//     public Button itemInfoCloseButton;
    
//     public bool isInventoryOpen = false;
//     public bool isDialogueOpen = false;
//     public bool isPuzzleOpen = false;
    
//     private void Awake()
//     {
//         if (Instance == null)
//         {
//             Instance = this;
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }
    
//     private void Start()
//     {
//         InitializeUI();
//         SetupEventListeners();
//     }
    
//     private void InitializeUI()
//     {
//         // Initialize all panels as closed
//         if (inventoryPanel != null)
//             inventoryPanel.SetActive(false);
//         if (dialoguePanel != null)
//             dialoguePanel.SetActive(false);
//         if (puzzlePanel != null)
//             puzzlePanel.SetActive(false);
//         if (tooltipPanel != null)
//             tooltipPanel.SetActive(false);
//         if (itemInfoPanel != null)
//             itemInfoPanel.SetActive(false);
//     }
    
//     private void SetupEventListeners()
//     {
//         if (inventoryToggleButton != null)
//         {
//             inventoryToggleButton.onClick.AddListener(ToggleInventory);
//         }
        
//         if (puzzleCloseButton != null)
//         {
//             puzzleCloseButton.onClick.AddListener(ClosePuzzle);
//         }
        
//         if (itemInfoCloseButton != null)
//         {
//             itemInfoCloseButton.onClick.AddListener(CloseItemInfo);
//         }
//     }
    
//     public void ToggleInventory()
//     {
//         ToggleInventory(!isInventoryOpen);
//     }
    
//     public void ToggleInventory(bool open)
//     {
//         isInventoryOpen = open;
        
//         if (inventoryPanel != null)
//         {
//             inventoryPanel.SetActive(isInventoryOpen);
//         }
        
//         if (GameManager.Instance != null)
//         {
//             GameManager.Instance.isInventoryOpen = isInventoryOpen;
//         }
//     }
    
//     public void ShowDialogue(string text, string[] buttonTexts = null, System.Action<int>[] buttonCallbacks = null)
//     {
//         isDialogueOpen = true;
        
//         if (dialoguePanel != null)
//         {
//             dialoguePanel.SetActive(true);
//         }
        
//         if (dialogueText != null)
//         {
//             dialogueText.text = text;
//         }
        
//         // Setup dialogue buttons
//         if (buttonTexts != null && dialogueButtons != null)
//         {
//             for (int i = 0; i < dialogueButtons.Length && i < buttonTexts.Length; i++)
//             {
//                 if (dialogueButtons[i] != null)
//                 {
//                     dialogueButtons[i].gameObject.SetActive(true);
                    
//                     if (dialogueButtonTexts[i] != null)
//                     {
//                         dialogueButtonTexts[i].text = buttonTexts[i];
//                     }
                    
//                     // Clear existing listeners and add new ones
//                     dialogueButtons[i].onClick.RemoveAllListeners();
//                     if (buttonCallbacks != null && i < buttonCallbacks.Length)
//                     {
//                         int buttonIndex = i; // Capture for closure
//                         dialogueButtons[i].onClick.AddListener(() => {
//                             buttonCallbacks[buttonIndex]?.Invoke(buttonIndex);
//                             CloseDialogue();
//                         });
//                     }
//                 }
//             }
//         }
//     }
    
//     public void CloseDialogue()
//     {
//         isDialogueOpen = false;
        
//         if (dialoguePanel != null)
//         {
//             dialoguePanel.SetActive(false);
//         }
//     }
    
//     public void ShowPuzzle(string title, string description)
//     {
//         isPuzzleOpen = true;
        
//         if (puzzlePanel != null)
//         {
//             puzzlePanel.SetActive(true);
//         }
        
//         if (puzzleTitle != null)
//         {
//             puzzleTitle.text = title;
//         }
        
//         if (puzzleDescription != null)
//         {
//             puzzleDescription.text = description;
//         }
        
//         if (GameManager.Instance != null)
//         {
//             GameManager.Instance.SetPuzzleMode(true);
//         }
//     }
    
//     public void ClosePuzzle()
//     {
//         isPuzzleOpen = false;
        
//         if (puzzlePanel != null)
//         {
//             puzzlePanel.SetActive(false);
//         }
        
//         if (GameManager.Instance != null)
//         {
//             GameManager.Instance.SetPuzzleMode(false);
//         }
//     }
    
//     public void ShowTooltip(string title, string description)
//     {
//         if (tooltipPanel != null)
//         {
//             tooltipPanel.SetActive(true);
            
//             if (tooltipTitle != null)
//             {
//                 tooltipTitle.text = title;
//             }
            
//             if (tooltipDescription != null)
//             {
//                 tooltipDescription.text = description;
//             }
//         }
//     }
    
//     public void HideTooltip()
//     {
//         if (tooltipPanel != null)
//         {
//             tooltipPanel.SetActive(false);
//         }
//     }
    
//     public void ShowItemInfo(InventoryItem item)
//     {
//         if (itemInfoPanel != null)
//         {
//             itemInfoPanel.SetActive(true);
            
//             if (itemInfoIcon != null && item.itemIcon != null)
//             {
//                 itemInfoIcon.sprite = item.itemIcon;
//             }
            
//             if (itemInfoName != null)
//             {
//                 itemInfoName.text = item.itemName;
//             }
            
//             if (itemInfoDescription != null)
//             {
//                 itemInfoDescription.text = item.description;
//             }
//         }
//     }
    
//     public void CloseItemInfo()
//     {
//         if (itemInfoPanel != null)
//         {
//             itemInfoPanel.SetActive(false);
//         }
//     }
    
//     public void ShowNotification(string message, float duration = 3f)
//     {
//         StartCoroutine(ShowNotificationCoroutine(message, duration));
//     }
    
//     private IEnumerator ShowNotificationCoroutine(string message, float duration)
//     {
//         // Create a simple notification (you can replace this with a proper notification system)
//         GameObject notification = new GameObject("Notification");
//         notification.transform.SetParent(transform);
        
//         Canvas canvas = notification.AddComponent<Canvas>();
//         canvas.overrideSorting = true;
//         canvas.sortingOrder = 1000;
        
//         CanvasScaler scaler = notification.AddComponent<CanvasScaler>();
//         scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
//         scaler.referenceResolution = new Vector2(1920, 1080);
        
//         GraphicRaycaster raycaster = notification.AddComponent<GraphicRaycaster>();
        
//         GameObject textObj = new GameObject("Text");
//         textObj.transform.SetParent(notification.transform);
        
//         TextMeshProUGUI text = textObj.AddComponent<TextMeshProUGUI>();
//         text.text = message;
//         text.fontSize = 24;
//         text.color = Color.white;
//         text.alignment = TextAlignmentOptions.Center;
        
//         RectTransform rectTransform = textObj.GetComponent<RectTransform>();
//         rectTransform.anchorMin = new Vector2(0.5f, 0.8f);
//         rectTransform.anchorMax = new Vector2(0.5f, 0.8f);
//         rectTransform.anchoredPosition = Vector2.zero;
//         rectTransform.sizeDelta = new Vector2(400, 100);
        
//         yield return new WaitForSeconds(duration);
        
//         Destroy(notification);
//     }
// }
