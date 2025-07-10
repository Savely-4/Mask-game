using UnityEngine;

namespace Runtime.InventorySystem 
{
    public interface IPickableItem : IItem
    {
        public ItemData ItemData { get; }
    }
}

