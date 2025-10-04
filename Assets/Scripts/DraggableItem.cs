// using UnityEngine;
// using UnityEngine.EventSystems;

// public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
// {
//     private Vector3 originalPosition;
//     private Transform originalParent;
//     private bool isDroppedCorrectly = false;

//     public string itemName; // Unique identifier for the item
//     public int sceneIndex; // Scene index for tracking in GameState

//     void Start()
//     {
//         originalPosition = transform.position;
//         originalParent = transform.parent;
//     }

//     public void OnBeginDrag(PointerEventData eventData)
//     {
//         if (isDroppedCorrectly) return; // Prevent dragging if already dropped correctly
//         originalPosition = transform.position;
//     }

//     public void OnDrag(PointerEventData eventData)
//     {
//         if (isDroppedCorrectly) return; // Prevent dragging if already dropped correctly

//         Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//         mouseWorldPosition.z = 0; // Ensure the object stays on the same Z-plane
//         transform.position = mouseWorldPosition;
//     }

//     public void OnEndDrag(PointerEventData eventData)
//     {
//         if (isDroppedCorrectly) return; // Prevent dragging if already dropped correctly

//         // Check if dropped on a valid drop zone
//         DropZone dropZone = GetDropZoneUnderMouse();
//         if (dropZone != null && dropZone.IsCorrectZoneForItem(itemName))
//         {
//             isDroppedCorrectly = true;
//             transform.position = dropZone.transform.position; // Snap to drop zone
//             GameState.Instance.MarkSuccessfulDrop(sceneIndex, itemName);
//         }
//         else
//         {
//             // Return to original position if not dropped correctly
//             transform.position = originalPosition;
//         }
//     }

//     private DropZone GetDropZoneUnderMouse()
//     {
//         RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
//         if (hit.collider != null)
//         {
//             return hit.collider.GetComponent<DropZone>();
//         }
//         return null;
//     }
// }