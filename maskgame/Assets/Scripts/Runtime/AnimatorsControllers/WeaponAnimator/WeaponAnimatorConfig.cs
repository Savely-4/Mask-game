using System.Collections.Generic;
using WeaponSystem;
using UnityEngine;
using Utils.CustomCollections;

[CreateAssetMenu(fileName = "WeaponAnimatorConfig", menuName = "Scriptable Objects/Animators/WeaponAnimatorConfig")]
public class WeaponAnimatorConfig : ScriptableObject
{
    [SerializeField] private List<WeaponStringsPair> _weaponAnimationsPair;
    
    public Dictionary<Weapon, WeaponAnimationsConfig> WeaponAnimations 
    {
        get 
        {
            Dictionary<Weapon, WeaponAnimationsConfig> dictionary = new();
            
            foreach (var pair in _weaponAnimationsPair)
            {
                if (!dictionary.ContainsKey(pair.Weapon))
                {
                    dictionary.Add(pair.Weapon, pair.AnimationsConfig);
                }
            }
            
            return dictionary;
        }
    }
}
