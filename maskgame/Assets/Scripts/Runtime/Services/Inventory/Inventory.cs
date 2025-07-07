using System.Collections.Generic;
using UnityEngine;
using Runtime.Configs;
using System;

namespace Runtime.InventorySystem 
{
    public class Inventory 
    {
        private readonly InventoryConfig _config;
        private List<InventorySlot> _slots = new();

        private InventorySlot _currentSelectedSlot;

        public int CurrentSpace { get; private set; }
        
        public InventorySlot CurrentSelectedSlot  
        {
            get => _currentSelectedSlot;
            
            private set 
            {
                _currentSelectedSlot = value;

                OnChangeCurrentSelectedSlot?.Invoke(_currentSelectedSlot);
            }
        }

        public event Action OnInventoryChanged;
        
        public event Action<ItemData> OnAddedItemInSlot;
        public event Action<ItemData> OnTryRemoveItemInSlot;
        public event Action OnRemovedItemInSlot;

        public event Action<InventorySlot> OnChangeCurrentSelectedSlot;
        
        public Inventory(InventoryConfig config)
        {
            _config = config;
            CurrentSpace = _config.Space;

            CreateSpace();

            CurrentSelectedSlot = _slots[0];
        }
        public bool TryAddItemInSlot(ItemData newItemData)
        {
            for (int i = 0; i < _slots.Count; i++) 
            {
                if (TryAddItemInSlotAt(newItemData, i)) 
                {
                    return true;
                }
            }
            
            return false;
        }
        
        public bool TryAddItemInSlotAt(ItemData newItemData, int index) 
        {
            if (_slots[index].IsEmpty || newItemData.StackCount > _slots[index].CountItems) 
            {
                if (_slots[index].TryAddItem(newItemData)) 
                {
                    OnAddedItemInSlot?.Invoke(newItemData);
                    OnInventoryChanged?.Invoke();
                    return true;
                }
            }

            return false;
        }
        
        public bool TryRemoveItemInSlot(ItemData newItemData) 
        {
            for (int i = 0; i < _slots.Count; i++) 
            {
                if (_slots[i].ItemData == newItemData) 
                {
                    return TryRemoveItemInSlotAt(i);
                }
            }

            return false;
        }
        
        public bool TryRemoveItemInSlotAt(int index) 
        {
            OnTryRemoveItemInSlot?.Invoke(_slots[index].ItemData);
            
            if (_slots[index].TryRemoveItem()) 
            {
                OnRemovedItemInSlot?.Invoke();
                OnInventoryChanged?.Invoke();
                return true;
            }

            return false;
        }
        
        public void SwitchSlot(int index) 
        {
            if (_slots.Count >= index) 
            {
                CurrentSelectedSlot = _slots[index];
            }
        }
        
        public void SetNewSpace(int newSpace) 
        {
            if (CurrentSpace < newSpace) 
            {
                for (int i = CurrentSpace + 1; i <= newSpace; i++) 
                {
                    _slots.Add(new InventorySlot()); 
                }
                
                Debug.Log($"Размер ивентаря увеличен на {newSpace} слотов");
                CurrentSpace = newSpace;
                OnInventoryChanged?.Invoke();
            }
        
            else if (CurrentSpace > newSpace) 
            {
                for (int i = CurrentSpace; i > newSpace; i--) 
                {
                    _slots.RemoveAt(i);
                }
                
                Debug.Log($"Размер ивентаря уменьшен на {newSpace} слотов");
                CurrentSpace = newSpace;
                OnInventoryChanged?.Invoke();
            }
        }
        
        private void CreateSpace() 
        {
            for (int i = 0; i < CurrentSpace; i++)
                    _slots.Add(new InventorySlot());
            
            OnInventoryChanged?.Invoke();
        }
    }
}

