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

        private int _currentSelectedSlotIndex;

        public int CurrentSpace { get; private set; }
        
        public int CurrentSelectedSlotIndex  
        {
            get => _currentSelectedSlotIndex;
            
            private set 
            {
                _currentSelectedSlotIndex = value;

                if (value == -1) return;

                OnChangeCurrentSelectedSlot?.Invoke(_currentSelectedSlotIndex);
            }
        }

        public event Action OnInventoryChanged;
        
        public event Action<IPickableItem, int> OnAddedItemInSlot;
        public event Action<int> OnRemovedItemInSlot;

        public event Action<int> OnChangeCurrentSelectedSlot;
        
        public Inventory(InventoryConfig config)
        {
            _config = config;
            CurrentSpace = _config.Space;
            
            CreateSpace();

            SwitchSlot(-1);

            Debug.Log("Инвентарь создался на " + CurrentSpace + " слотов");
        }
        
        public void AddItemInSlot(IPickableItem newItem)
        {
            for (int i = 0; i < _slots.Count; i++) 
            {
                if (TryAddItemInSlotAt(newItem, i)) 
                {
                    break;
                }
            }
        }
        
        public bool TryAddItemInSlotAt(IPickableItem newItem, int index) 
        {  
            if (_slots[index].TryAddItem(newItem)) 
            {
                if(_currentSelectedSlotIndex == -1)
                    SwitchSlot(0);

                OnAddedItemInSlot?.Invoke(newItem, index);
                OnInventoryChanged?.Invoke();

                Debug.Log("Положили в инвентарь");
                return true;
            }
            
            return false;
        }

        public bool TryRemoveItemInCurrentSlot() 
        {
            return RemoveItemInSlotAt(CurrentSelectedSlotIndex);
        }
        
        public bool TryRemoveItemInSlot(IPickableItem item) 
        {
            for (int i = 0; i < _slots.Count; i++) 
            {
                for (int j = _slots[i].Items.Count; j >= 0; j--) 
                {
                    if (_slots[i].Items[j] == item) 
                    {
                        RemoveItemInSlotAt(i);
                        return true;
                    }
                    
                }
                
            }

            return false;
        }
        
        public bool RemoveItemInSlotAt(int slotIndex) 
        {
            if (_slots[slotIndex].IsEmpty) return false;

            if (_slots[slotIndex].TryRemoveItem()) 
            { 
                OnRemovedItemInSlot?.Invoke(slotIndex);
                OnInventoryChanged?.Invoke();

                return true;
            }
            
            return false;
        }
        
        public IPickableItem GetItemInSlotAt(int slotIndex) 
        {
            Debug.Log(_slots.Count);
            for (int i = _slots[slotIndex].Items.Count - 1; i >= 0; i--) 
            {
                if (_slots[slotIndex].Items[i] != null) 
                {
                    return _slots[slotIndex].Items[i];
                }
            }
            return null;
        }
        
        public void SwitchSlot(int index) 
        {
            if (_slots.Count >= index) 
            {
                CurrentSelectedSlotIndex = index;
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

