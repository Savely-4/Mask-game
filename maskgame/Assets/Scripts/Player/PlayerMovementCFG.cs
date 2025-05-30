using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovementConfig", menuName = "Scriptable Objects/Player/PlayerMovementConfig")]
public class PlayerMovementCFG : ScriptableObject
{
    [field: SerializeField] public float Speed { get; private set; } = 5f;
    [field: SerializeField] public float SprintSpeed { get; private set; }  = 10f;
    [field: SerializeField] public float JumpDistance { get; private set; } = 1.7f;
}
