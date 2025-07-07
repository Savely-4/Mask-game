
namespace Runtime.InventorySystem 
{
    public class InventorySlot
    {
        public ItemData ItemData { get; private set; }

        public int CountItems { get; private set; }

        public bool IsEmpty => ItemData == null;

        public bool TryAddItem(ItemData newItemData)
        {   
            if (ItemData == null)
                ItemData = newItemData;
                
            CountItems++;
            
            return true;
        }
        
        public bool TryRemoveItem() 
        {
            if (ItemData != null)
            {
                if (CountItems <= 0) 
                {
                    ItemData = null;
                }
                else 
                {
                    CountItems--;
                }
                
                return true;
            } 
            
            return false;
        }

        public void ClearSlot()
        {
            ItemData = null;
            CountItems = 0;
        }
    }
}

