
using System.Collections.Generic;

namespace Runtime.InventorySystem 
{
    public class InventorySlot
    {
        public ItemData ItemData { get; private set; }
        public List<IPickableItem> Items { get; private set; } = new();
        public int CountItems { get; private set; }

        public bool IsEmpty => ItemData == null;

        public bool TryAddItem(IPickableItem item, ItemData newItemData)
        {
            if (ItemData != null)
            {
                ItemData = newItemData;
            }
            
            Items.Add(item);
            CountItems++;
            return true;
        }
        
        public bool TryRemoveItem() 
        {
            if (ItemData != null)
            {
                if (CountItems <= 0) 
                {
                    Items.Clear();
                    ItemData = null;
                }
                else 
                {
                    CountItems--;
                    Items.RemoveAt(CountItems - 1);
                }
                return true;
            } 
            return false;
        }

        public void ClearSlot()
        {
            ItemData = null;
            Items.Clear();
            CountItems = 0;
        }
    }
}

