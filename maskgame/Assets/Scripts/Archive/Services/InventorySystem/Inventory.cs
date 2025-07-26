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

                OnChangeCurrentSelectedSlot?.Invoke();
            }
        }

        public event Action<IPickableItem, int> OnAddedItemInSlot;
        public event Action<int> OnRemovedItemInSlot;

        public event Action OnChangeCurrentSelectedSlot;
        
        public Inventory(InventoryConfig config)
        {
            _config = config;
            CurrentSpace = _config.Space;
            
            AddSlots(CurrentSpace);

            SwitchSlot(0);
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
                /*/if(_currentSelectedSlotIndex == -1)
                    SwitchSlot(0);/*/
                    
                if (_currentSelectedSlotIndex == index) 
                {
                    OnChangeCurrentSelectedSlot?.Invoke();
                }

                OnAddedItemInSlot?.Invoke(newItem, index);

                //Debug.Log("Положили в инвентарь");
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
                for (int j = _slots[i].Items.Count - 1; j >= 0; j--) 
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
                return true;
            }
            
            return false;
        }
        
        public IPickableItem GetItemInSlotAt(int slotIndex) 
        {
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
            if (_slots.Count > index && index > 0 && index != CurrentSelectedSlotIndex) 
            {
                CurrentSelectedSlotIndex = index;
            }
        }
        
        public void SetNewSpace(int newSpace)
        {
            if (newSpace > CurrentSpace)
            {
                AddSlots(newSpace - CurrentSpace);
            }
            else if (newSpace < CurrentSpace)
            {
                RemoveSlots(CurrentSpace - newSpace);
            }
            
            CurrentSpace = newSpace;
        }

        private void AddSlots(int count)
        {
            for (int i = 0; i < count; i++)
                _slots.Add(new InventorySlot());
                
            Debug.Log($"Инвентарь увеличен на {count} слотов");
        }

        private void RemoveSlots(int count)
        {
            for (int i = 0; i < count; i++)
                _slots.RemoveAt(_slots.Count - 1);
                
            Debug.Log($"Инвентарь уменьшен на {count} слотов");
        }
    }
}

