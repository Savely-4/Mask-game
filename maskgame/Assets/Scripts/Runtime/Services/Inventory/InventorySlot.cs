using System;
using UnityEngine;

namespace Runtime.InventorySystem 
{
    public class InventorySlot
    {
        private ItemData _item;
        public ItemData Item  
        {
            get => _item;
            
            private set 
            {
                _item = value;
                OnSlotChanged?.Invoke(this);
            }
        }
        private int _count;

        public event Action<InventorySlot> OnSlotChanged;
        public void AddItem(ItemData newItem)
        {
            if (Item == null)
            {
                Item = newItem;
                _count = 1;
            }
            else if (Item == newItem && Item.IsStackable)
            {
                _count++;
            }
        }

        public void ClearSlot()
        {
            Item = null;
            _count = 0;
        }
    }
}

