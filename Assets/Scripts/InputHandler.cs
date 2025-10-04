// using UnityEngine;

// public class InputHandler : MonoBehaviour
// {
//     [Header("Input Settings")]
//     public float interactionRange = 2f;
//     public LayerMask interactiveLayer = -1;
    
//     private Camera playerCamera;
//     private InteractiveItem lastHoveredItem;
    
//     private void Start()
//     {
//         playerCamera = Camera.main;
//         if (playerCamera == null)
//         {
//             playerCamera = FindObjectOfType<Camera>();
//         }
//     }
    
//     private void Update()
//     {
//         HandleMouseInput();
//         HandleKeyboardInput();
//     }
    
//     private void HandleMouseInput()
//     {
//         Vector3 mousePosition = Input.mousePosition;
//         Vector3 worldPosition = playerCamera.ScreenToWorldPoint(mousePosition);
//         worldPosition.z = 0; // For 2D games
        
//         // Check for interactive items under mouse
//         Collider2D hitCollider = Physics2D.OverlapPoint(worldPosition, interactiveLayer);
//         InteractiveItem currentItem = null;
        
//         if (hitCollider != null)
//         {
//             currentItem = hitCollider.GetComponent<InteractiveItem>();
//         }
        
//         // Handle hover events
//         if (currentItem != lastHoveredItem)
//         {
//             if (lastHoveredItem != null)
//             {
//                 // Exit previous item
//                 lastHoveredItem.OnPointerExit(null);
//             }
            
//             if (currentItem != null)
//             {
//                 // Enter new item
//                 currentItem.OnPointerEnter(null);
//             }
            
//             lastHoveredItem = currentItem;
//         }
        
//         // Handle clicks
//         if (Input.GetMouseButtonDown(0))
//         {
//             if (currentItem != null)
//             {
//                 currentItem.OnPointerClick(null);
//             }
//         }
//     }
    
//     private void HandleKeyboardInput()
//     {
//         // Inventory toggle
//         if (Input.GetKeyDown(KeyCode.I))
//         {
//             if (UIManager.Instance != null)
//             {
//                 UIManager.Instance.ToggleInventory();
//             }
//         }
        
//         // Escape key
//         if (Input.GetKeyDown(KeyCode.Escape))
//         {
//             HandleEscapeKey();
//         }
        
//         // Debug keys
//         if (Input.GetKeyDown(KeyCode.R))
//         {
//             ResetCurrentScene();
//         }
//     }
    
//     private void HandleEscapeKey()
//     {
//         if (UIManager.Instance != null)
//         {
//             // Close any open UI panels
//             if (UIManager.Instance.isInventoryOpen)
//             {
//                 UIManager.Instance.ToggleInventory(false);
//             }
//             else if (UIManager.Instance.isPuzzleOpen)
//             {
//                 UIManager.Instance.ClosePuzzle();
//             }
//             else if (UIManager.Instance.isDialogueOpen)
//             {
//                 UIManager.Instance.CloseDialogue();
//             }
//         }
//     }
    
//     private void ResetCurrentScene()
//     {
//         SceneController sceneController = FindObjectOfType<SceneController>();
//         if (sceneController != null)
//         {
//             sceneController.ResetScene();
//             Debug.Log("Scene reset!");
//         }
//     }
// }
