
namespace Runtime.InventorySystem 
{
    public class InventorySlot
    {
        public IPickableItem Item { get; private set; }

        public int CountItems { get; private set; }

        public bool IsEmpty => Item == null;

        public bool TryAddItem(IPickableItem newItem)
        {   
            if (Item == null)
                Item = newItem;
                
            CountItems++;
            
            return true;
        }
        
        public bool TryRemoveItem() 
        {
            if (Item != null)
            {
                if (CountItems <= 0) 
                {
                    Item = null;
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
            Item = null;
            CountItems = 0;
        }
    }
}

