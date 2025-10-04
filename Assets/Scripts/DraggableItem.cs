using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 originalPosition;
    private Transform originalParent;
    private bool isDroppedCorrectly = false;

    public DraggableItemConfig.ItemZonePair itemZonePair; // Reference to the item-zone pair from the config
    public int sceneIndex; // Scene index for tracking in GameState
    public UnityEvent<string> OnItemDropped;
    void Start()
    {
        // Initialize the item's position and name from the config
        if (itemZonePair != null)
        {
            originalPosition = itemZonePair.initialPosition;
            transform.position = originalPosition;
        }
        else
        {
            Debug.LogError($"DraggableItem on {gameObject.name} is missing its ItemZonePair configuration!");
        }

        // Initialize the UnityEvent if not set in the Inspector
        if (OnItemDropped == null)
        {
            OnItemDropped = new UnityEvent<string>();
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isDroppedCorrectly) return; // Prevent dragging if already dropped correctly
        originalPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDroppedCorrectly) return; // Prevent dragging if already dropped correctly

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0; // Ensure the object stays on the same Z-plane
        transform.position = mouseWorldPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isDroppedCorrectly) return; // Prevent dragging if already dropped correctly

        // Check if dropped on a valid drop zone
        Drop dropZone = GetDropZoneUnderMouse();
        if (dropZone != null && dropZone.zoneName == itemZonePair.zoneName && dropZone.IsCorrectZoneForItem(transform.position))
        {
            isDroppedCorrectly = true;
            transform.position = dropZone.transform.position; // Snap to drop zone
            GameState.Instance.MarkSuccessfulDrop(sceneIndex, itemZonePair.itemName);

            OnItemDropped?.Invoke(itemZonePair.itemName);
        }
        else
        {
            // Return to original position if not dropped correctly
            transform.position = originalPosition;
        }
    }

    private Drop GetDropZoneUnderMouse()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null)
        {
            return hit.collider.GetComponent<Drop>();
        }
        return null;
    }
}