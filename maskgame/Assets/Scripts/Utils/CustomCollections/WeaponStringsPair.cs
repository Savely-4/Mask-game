using System;
using UnityEngine;
using WeaponSystem;

[Serializable]
public class WeaponStringsPair
{
    [Header("Key")] public Weapon Weapon;
    [Header("Value")] public WeaponAnimationsConfig AnimationsConfig;
}
