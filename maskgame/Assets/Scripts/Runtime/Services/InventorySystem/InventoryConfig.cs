using UnityEngine;

namespace Runtime.Configs 
{
    [CreateAssetMenu(fileName = "InventoryConfig", menuName = "Scriptable Objects/Inventory/InventoryConfig")]
    public class InventoryConfig : ScriptableObject
    {
        [field: SerializeField] public int Space {get; private set;}
    }
}

