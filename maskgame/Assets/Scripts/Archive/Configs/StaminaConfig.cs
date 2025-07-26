using UnityEngine;

namespace Runtime.Configs
{
    [CreateAssetMenu(fileName = "StaminaSettings", menuName = "Scriptable Objects/Player/StaminaSettings")]
    public class StaminaConfig : ScriptableObject
    {
        [field: SerializeField] public float StaminaMax { get; private set; } = 100f;
    }
}