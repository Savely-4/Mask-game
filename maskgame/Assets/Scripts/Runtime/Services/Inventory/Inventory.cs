using System.Collections.Generic;
using UnityEngine;
using Runtime.Configs;
using System;

namespace Runtime.InventorySystem 
{
    public class Inventory : MonoBehaviour
    {
        private readonly InventoryConfig _config;
        private List<InventorySlot> _slots = new();

        public int CurrentSpace { get; private set; }

        public event Action OnInventoryChanged;
        public Inventory(InventoryConfig config)
        {
            _config = config;
            CurrentSpace = _config.Space;
            
            for (int i = 0; i < CurrentSpace; i++)
                _slots.Add(new InventorySlot());
        }
        public bool TryAddItemInSlot(ItemData itemToAdd)
        {
           

            Debug.Log("Inventory is full!");
            return false;
        }
    }
}

