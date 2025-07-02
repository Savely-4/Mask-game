using UnityEngine;

namespace Runtime.Configs
{
    [CreateAssetMenu(fileName = "PlayerInputKeyboardConfig", menuName = "Scriptable Objects/Player/PlayerInputKeyboardConfig")]
    public class PlayerInputKeyboardConfig : ScriptableObject
    {
        [field: SerializeField] public KeyCode PrimaryAttackKey { get; private set; } = KeyCode.Mouse0;
        [field: SerializeField] public KeyCode AlternateAttackKey { get; private set; } = KeyCode.Mouse1;
    }
}
