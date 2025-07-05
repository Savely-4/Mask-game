using UnityEngine;

namespace Runtime.Configs
{
    public abstract class ItemInteractorConfig : ScriptableObject
    {
        [field: SerializeField] public float PickupRate { get; private set; } = 0.2f;
        [field: SerializeField] public float DropRate { get; private set; } = 0.2f;
        [field: SerializeField] public float PickupDistance { get; private set; } = 10f;
        [field: SerializeField] public float PickupForce { get; private set; } = 5f;
    }
}

