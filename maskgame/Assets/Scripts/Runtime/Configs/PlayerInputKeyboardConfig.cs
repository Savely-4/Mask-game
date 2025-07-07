using UnityEngine;

namespace Runtime.Configs
{
    [CreateAssetMenu(fileName = "PlayerInputKeyboardConfig", menuName = "Scriptable Objects/Player/PlayerInputKeyboardConfig")]
    public class PlayerInputKeyboardConfig : ScriptableObject
    {
        [Header("Combat inputs")]
        [field: SerializeField] public KeyCode PrimaryAttackKey { get; private set; } = KeyCode.Mouse0;
        [field: SerializeField] public KeyCode AlternateAttackKey { get; private set; } = KeyCode.Mouse1;
        
        [Header("Interactor inputs")]
        [field: SerializeField] public KeyCode PickupKey { get; private set; } = KeyCode.E;
        [field: SerializeField] public KeyCode DropKey { get; private set; } = KeyCode.Q;
        
        [Header("Axis")]
        [field: SerializeField] public string MouseXAxis { get; private set; } = "Mouse X";
        [field: SerializeField] public string MouseYAxis { get; private set; } = "Mouse Y";
        [field: SerializeField] public string HorizontalAxis { get; private set; } = "Horizontal";
        [field: SerializeField] public string VerticalAxis { get; private set; } = "Vertical";
        
        [Header("Movement inputs")]
        [field: SerializeField] public KeyCode JumpKey { get; private set; } = KeyCode.Space;
        [field: SerializeField] public KeyCode SprintKey { get; private set; } = KeyCode.LeftShift;

    }
}
