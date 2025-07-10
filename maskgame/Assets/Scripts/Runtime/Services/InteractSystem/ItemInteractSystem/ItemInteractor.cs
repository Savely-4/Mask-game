using System;
using Runtime.InventorySystem;
using UnityEngine;

namespace Runtime.Services.InteractSystem
{
    public abstract class ItemInteractor 
    {
        private readonly ItemInteractorConfig _config;
        
        protected float _lastPickupTime = Mathf.NegativeInfinity;
        protected float _lastDropTime = Mathf.NegativeInfinity;

        public event Action<IPickableItem> OnPickupItem;
        public event Func<bool> OnTryDropItem;
        
        public ItemInteractor(ItemInteractorConfig config)
        {
            _config = config;
        }

        public virtual void PickupUpdate(Vector3 pickupPoint, Vector3 direction, bool isPickup) 
        {
            if (isPickup && IsTimePassed(_config.PickupRate, ref _lastPickupTime))
            {  
                if (Physics.Raycast(pickupPoint, direction.normalized, out RaycastHit hit, _config.PickupDistance)) 
                {                
                    if (hit.collider.TryGetComponent<IPickableItem>(out var pickableItem)) 
                    {
                        OnPickupItem?.Invoke(pickableItem);
                        Debug.Log("Мы попали в предмет лучем");
                    }
                }
            }
        }
        
        public virtual void DropUpdate(bool isDrop) 
        {
            if (isDrop)
            {
                if (IsTimePassed(_config.DropRate, ref _lastDropTime)) 
                {
                    OnTryDropItem?.Invoke();
                }
            }
        }

        protected virtual bool IsTimePassed(float interactRate, ref float lastActionTime)
        {
            if (Time.time >= interactRate + lastActionTime) 
            {
                lastActionTime = Time.time;
                return true;
            }
            return false;
        }
    }
}

