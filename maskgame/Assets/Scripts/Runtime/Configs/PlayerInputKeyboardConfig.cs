using UnityEngine;

namespace Runtime.Configs
{
    [CreateAssetMenu(fileName = "PlayerInputKeyboardConfig", menuName = "Scriptable Objects/Player/PlayerInputKeyboardConfig")]
    public class PlayerInputKeyboardConfig : ScriptableObject
    {
        [field: SerializeField] public KeyCode PrimaryAttackKey { get; private set; } = KeyCode.Mouse0;
        [field: SerializeField] public KeyCode AlternateAttackKey { get; private set; } = KeyCode.Mouse1;
        [field: SerializeField] public string MouseXAxis { get; private set; } = "Mouse X";
        [field: SerializeField] public string MouseYAxis { get; private set; } = "Mouse Y";
        [field: SerializeField] public string HorizontalAxis { get; private set; } = "Horizontal";
        [field: SerializeField] public string VerticalAxis { get; private set; } = "Vertical";
        [field: SerializeField] public KeyCode JumpKey { get; private set; } = KeyCode.Space;
        [field: SerializeField] public KeyCode SprintKey { get; private set; } = KeyCode.LeftShift;

    }
}
