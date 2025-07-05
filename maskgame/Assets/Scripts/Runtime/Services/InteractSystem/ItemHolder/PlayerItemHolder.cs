using Runtime.InventorySystem;
using UnityEngine;

public class PlayerItemHolder
{
    private readonly Transform _holderPoint;

    private ItemData _currentItemData;
    
    public PlayerItemHolder(Transform holderPoint)
    {
        _holderPoint = holderPoint;
    }


    public void Equip(ItemData itemData)
    {
        
    }
    
    public void UnEquip() 
    {
        
    }
}
