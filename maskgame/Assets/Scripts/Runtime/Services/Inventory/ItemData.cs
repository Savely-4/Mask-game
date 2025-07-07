using UnityEngine;

namespace Runtime.InventorySystem 
{
    public enum ItemType { Weapon, Armor, Potion, Misc }

    [CreateAssetMenu(fileName = "NewItem", menuName = "Scriptable Objects/Inventory/Item")] 
    public class ItemData : ScriptableObject
    {
        public string ItemName;
        public ItemType ItemType;
        public int StackCount;
        public GameObject Prefab;
    }
}
