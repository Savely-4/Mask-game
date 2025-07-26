using System;
using Runtime.Entities.WeaponSystem;
using UnityEngine;

namespace Utils.CustomCollections
{
    [Serializable]
    public class WeaponStringsPair
    {
        [Header("Key")] public Weapon Weapon;
        [Header("Value")] public WeaponAnimationsConfig AnimationsConfig;
    }
}
