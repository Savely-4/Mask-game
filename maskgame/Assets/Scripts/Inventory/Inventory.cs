using UnityEngine;
using Runtime.Configs;

namespace Runtime.InventorySystem 
{
    public class Inventory
    {
        private readonly InventoryConfig _config;
        
        public Inventory(InventoryConfig config)
        {
            _config = config;
        }
    }
}

