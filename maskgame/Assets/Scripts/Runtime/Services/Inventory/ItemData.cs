using UnityEngine;

namespace Runtime.InventorySystem 
{
    public enum ItemType { Weapon, Armor, Potion, Misc }

    public class ItemUIData : ScriptableObject
    {
        public string ItemName;
        public Sprite Icon;
        public ItemType ItemType;
        public bool IsStackable;
    }
}
