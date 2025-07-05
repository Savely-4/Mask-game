using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponAnimationsConfig : ScriptableObject
{
    [field: SerializeField] public List<string> MainAttackAnimations { get; private set; }
}
