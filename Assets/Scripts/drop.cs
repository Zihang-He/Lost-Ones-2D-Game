using UnityEngine;

public class Drop : MonoBehaviour
{
    public string zoneName; // Name of the drop zone
    public float acceptableDistance = 1f;
    public bool IsCorrectZoneForItem(Vector3 itemPosition)
    {
        // Check if the item is within the acceptable distance of the zone
        float distance = Vector3.Distance(transform.position, itemPosition);
        return distance <= acceptableDistance;
    }
}