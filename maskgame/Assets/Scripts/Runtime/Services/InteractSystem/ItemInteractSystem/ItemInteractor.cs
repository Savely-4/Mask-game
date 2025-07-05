using System;
using Runtime.Configs;
using Runtime.InventorySystem;
using UnityEngine;

namespace Runtime.InteractSystem 
{
    public abstract class ItemInteractor 
    {
        private readonly ItemInteractorConfig _config;
        
        private IPickableItem _currentPickableItem;
        
        protected float _lastPickupTime = Mathf.NegativeInfinity;
        protected float _lastDropTime = Mathf.NegativeInfinity;

        public event Action<IPickableItem> OnPickupItem;
        public event Action<IPickableItem> OnDropItem;
        
        public ItemInteractor(ItemInteractorConfig config)
        {
            _config = config;
        }

        public virtual void PickupUpdate(Vector3 pickupPoint, bool isPickup) 
        {
            if (isPickup && PickupTimePassed())
            {   
                if (Physics.Raycast(pickupPoint,  pickupPoint.normalized, out RaycastHit hit, _config.PickupDistance)) 
                {                
                    if (hit.collider.TryGetComponent<IPickableItem>(out var pickableItem)) 
                    {
                        _currentPickableItem = pickableItem;
                        OnPickupItem?.Invoke(pickableItem);
                    }
                }
            }
        }
        public virtual void DropUpdate(Vector3 dropPoint, bool isDrop) 
        {
            if (isDrop) 
            {
                if (_currentPickableItem != null && DropTimePassed()) 
                {
                    var mono = _currentPickableItem as MonoBehaviour;
                    
                    if (mono != null && mono.TryGetComponent<Rigidbody>(out var rigidbody))
                    {
                        rigidbody.AddForce(dropPoint.normalized * _config.PickupForce, ForceMode.Impulse);
                        
                        OnDropItem?.Invoke(_currentPickableItem);
                        
                        _currentPickableItem = null;
                    }
                }
            }
        }
        
        protected virtual bool PickupTimePassed() 
        {
            if (Time.time >= _config.PickupRate + _lastPickupTime) 
            {
                _lastPickupTime = Time.time;
                return true;
            }
            
            return false;
        }
        
        protected virtual bool DropTimePassed() 
        {
            if (Time.time >= _config.DropRate + _lastDropTime) 
            {
                _lastDropTime = Time.time;
                return true;
            }
            
            return false;
        }
    }
}

