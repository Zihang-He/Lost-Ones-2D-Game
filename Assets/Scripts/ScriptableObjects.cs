// filepath: /Users/hezihang/Desktop/college/grad/CSE218/My project/Assets/Scripts/DraggableItemConfig.cs
using UnityEngine;

[CreateAssetMenu(fileName = "DraggableItemConfig", menuName = "Game/DraggableItemConfig")]
public class DraggableItemConfig : ScriptableObject
{
    [System.Serializable]
    public class ItemZonePair
    {
        public string itemName; // Name of the draggable item
        public Transform itemTransform; // Reference to the draggable item's transform
        public Transform correctZoneTransform; // Reference to the correct drop zone's transform
    }

    public ItemZonePair[] itemZonePairs; // Array of item-zone mappings
}