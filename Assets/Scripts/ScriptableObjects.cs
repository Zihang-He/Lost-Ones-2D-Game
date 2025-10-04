using UnityEngine;

[CreateAssetMenu(fileName = "DraggableItemConfig", menuName = "Game/DraggableItemConfig")]
public class DraggableItemConfig : ScriptableObject
{
    [System.Serializable]
    public class ItemZonePair
    {
        public string itemName; // Name of the draggable item
        public string zoneName; // Name of the correct drop zone
        public Vector3 initialPosition; // Initial position of the draggable item
    }

    public ItemZonePair[] itemZonePairs; // Array of item-zone mappings
}