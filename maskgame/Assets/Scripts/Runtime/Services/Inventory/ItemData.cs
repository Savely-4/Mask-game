using UnityEngine;

namespace Runtime.InventorySystem 
{
    public enum ItemType { Weapon, Armor, Potion, Misc }

    [CreateAssetMenu(fileName = "NewItem", menuName = "Scriptable Objects/Inventory/Item")] 
    public class ItemData : ScriptableObject
    {
        public string ItemName;
        public Sprite Icon;
        public ItemType ItemType;
        public bool IsStackable;
        public GameObject Prefab;
    }
}
