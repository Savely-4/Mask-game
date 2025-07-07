using Runtime.InventorySystem;
using UnityEngine;

public class PlayerItemHolder
{
    private readonly Transform _holderPoint;

    private IPickableItem _currentObjInHand;
    
    public PlayerItemHolder(Transform holderPoint)
    {
        _holderPoint = holderPoint;
    }


    public void Equip(IPickableItem pickableItem)
    {
        var mono = pickableItem as MonoBehaviour;
        
        if (mono != null) 
        {
            _currentObjInHand = pickableItem;
            mono.transform.SetParent(_holderPoint);
            mono.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
    }
    
    public void UnEquip() 
    {
       
    }
}
