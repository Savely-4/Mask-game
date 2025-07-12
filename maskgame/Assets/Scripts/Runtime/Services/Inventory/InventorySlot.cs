
using System.Collections.Generic;

namespace Runtime.InventorySystem 
{
    public class InventorySlot
    {
        public ItemData ItemData { get; private set; }
        public List<IPickableItem> Items { get; private set; } = new();
        public int CountItems { get; private set; }

        public bool IsEmpty => ItemData == null;

        public bool TryAddItem(IPickableItem item)
        {
            if (IsEmpty || item.ItemData.StackCount > CountItems)
            {
                ItemData = item.ItemData;
                CountItems++;
                Items.Add(item);
                return true;
            }
            
            return false;
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
                    Items.RemoveAt(--CountItems);

                    if(Items.Count == 0)
                        ItemData = null;
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

