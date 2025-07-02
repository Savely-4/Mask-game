using System;
using UnityEngine;

[Serializable]
public class WeaponStringsPair
{
    [Header("Key")] public Weapon Weapon;
    [Header("Value")] public WeaponAnimationsConfig AnimationsConfig;
}
