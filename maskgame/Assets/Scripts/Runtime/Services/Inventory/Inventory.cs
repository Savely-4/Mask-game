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

        public int CurrentSpace { get; private set; }

        public event Action OnInventoryChanged;
        public event Action<IPickableItem, InventorySlot> OnAddedItemInSlot;
        public event Action<InventorySlot> OnRemovedItemInSlot;
        
        public Inventory(InventoryConfig config)
        {
            _config = config;
            CurrentSpace = _config.Space;

            CreateSpace();
        }
        public bool TryAddItemInSlot(IPickableItem newItem)
        {
            for (int i = 0; i < _slots.Count; i++) 
            {
                if (_slots[i].IsEmpty) 
                {
                    if (_slots[i].TryAddItem(newItem)) 
                    {
                        OnAddedItemInSlot?.Invoke(newItem, _slots[i]);
                        return true;
                    }
                    
                }
            }

            return false;
        }
        
        public bool TryAddItemInSlotAt(IPickableItem newItem, int index) 
        {
            if (_slots[index].IsEmpty) 
            {
                if (_slots[index].TryAddItem(newItem)) 
                {
                    OnAddedItemInSlot?.Invoke(newItem, _slots[index]);
                    return true;
                }
                    
            }

            return false;
        }
        
        public bool TryRemoveItemSlot(IPickableItem newItem) 
        {
            for (int i = 0; i < _slots.Count; i++) 
            {
                if (_slots[i].Item == newItem) 
                {
                    if (_slots[i].TryRemoveItem()) 
                    {
                        OnRemovedItemInSlot?.Invoke(_slots[i]);
                        return true;
                    }
                    
                }
            }

            return false;
        }
        
        public bool TryRemoveItemSlotAt(int index) 
        {
            if (_slots[index].TryRemoveItem()) 
            {
                OnRemovedItemInSlot?.Invoke(_slots[index]);
                return true;
            }

            return false;
        }
        
        public void SetSpace(int newSpace) 
        {
            if (CurrentSpace < newSpace) 
            {
                for (int i = CurrentSpace + 1; i <= newSpace; i++) 
                {
                    _slots.Add(new InventorySlot()); 
                }
                
                Debug.Log($"Размер ивентаря увеличен на {newSpace} слотов");
                CurrentSpace = newSpace;
            }
        
            else if (CurrentSpace > newSpace) 
            {
                for (int i = CurrentSpace; i > newSpace; i--) 
                {
                    _slots.RemoveAt(i);
                }
                
                Debug.Log($"Размер ивентаря уменьшен на {newSpace} слотов");
                CurrentSpace = newSpace;
            }
        }
        
        private void CreateSpace() 
        {
            for (int i = 0; i < CurrentSpace; i++)
                    _slots.Add(new InventorySlot());
        }
    }
}

