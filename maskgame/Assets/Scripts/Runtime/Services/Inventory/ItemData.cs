using UnityEngine;

namespace Runtime.InventorySystem 
{
    public enum ItemType { Weapon, Armor, Potion, Misc }

    public abstract class ItemData : ScriptableObject
    {
        public string ItemName;
        public Sprite Icon;
        public ItemType ItemType;
        public bool IsStackable;

        public virtual void Use()
        {
            Debug.Log("Using: " + ItemName);
        }
    }
}
