using System.Collections.Generic;
using UnityEngine;
using Runtime.Configs;
using System;
using Runtime.Services.InteractSystem;

namespace Runtime.InventorySystem 
{
    public class Inventory 
    {
        private readonly InventoryConfig _config;
        private List<InventorySlot> _slots = new();

        private InventorySlot _currentSelectedSlot;
        private int _currentSelectedSlotIndex;

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

            SwitchSlot(0);
        }
        
        public void AddItemInSlot(IPickableItem newItem)
        {
            for (int i = 0; i < _slots.Count; i++) 
            {
                AddItemInSlotAt(newItem, i);
            }
        }
        
        public void AddItemInSlotAt(IPickableItem newItem, int index) 
        {
            if (_slots[index].IsEmpty || newItem.ItemData.StackCount > _slots[index].CountItems) 
            {
                if (_slots[index].TryAddItem(newItem, newItem.ItemData)) 
                {
                    OnAddedItemInSlot?.Invoke(newItem.ItemData);
                    OnInventoryChanged?.Invoke();
                }
            }
        }
        
        public void RemoveItemInSlot() 
        {
            for (int i = 0; i < _slots.Count; i++) 
            {
                RemoveItemInSlotAt(_currentSelectedSlotIndex);
            }
        }
        
        public void RemoveItemInSlotAt(int index) 
        {
            OnTryRemoveItemInSlot?.Invoke(_slots[index].ItemData);
            
            if (_slots[index].TryRemoveItem()) 
            {
                OnRemovedItemInSlot?.Invoke();
                OnInventoryChanged?.Invoke();
            }
        }
        
        public void SwitchSlot(int index) 
        {
            if (_slots.Count >= index) 
            {
                CurrentSelectedSlot = _slots[index];
                _currentSelectedSlotIndex = index;
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

