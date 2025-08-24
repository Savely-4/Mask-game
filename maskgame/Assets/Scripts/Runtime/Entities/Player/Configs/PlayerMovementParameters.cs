using UnityEngine;

namespace Runtime.Entities.Player
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Player/Movement Parameters")]
    public class PlayerMovementParameters : ScriptableObject
    {
        [field: Header("Movement parameters")]
        [field: SerializeField] public float Speed { get; private set; } = 4f;
        [field: SerializeField] public float SprintSpeed { get; private set; } = 8f;
        [field: SerializeField] public float Acceleration { get; private set; } = 50f;

        [field: Header("Jump Parameters")]
        [field: SerializeField] public float JumpHeight { get; private set; } = 10;
        [field: SerializeField] public int MaxJumps { get; private set; } = 2;

        [field: Header("Gravity Parameters")]
        [field: SerializeField] public float GravityValue { get; private set; } = -9.81f;
        [field: SerializeField] public float GravityRiseMultiplier { get; private set; } = 1;
        [field: SerializeField] public float GravityDescendMultiplier { get; private set; } = 1;
    }
}