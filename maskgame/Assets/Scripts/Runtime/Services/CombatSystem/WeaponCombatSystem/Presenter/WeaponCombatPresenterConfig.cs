using System;
using System.Collections.Generic;
using Runtime.Entities.WeaponSystem;
using UnityEngine;
using Utils.CustomCollections;

namespace Runtime.Services.CombatSystem
{
    [CreateAssetMenu(fileName = "WeaponCombatPresenterConfig", menuName = "Scriptable Objects/Combat/Weapon/WeaponCombatPresenterConfig")]
    public class WeaponCombatPresenterConfig : ScriptableObject
    {
        //[field: SerializeField] public WeaponCombatViewConfig WeaponCombatViewConfig { get; private set; }
        
        [SerializeField] private List<WeaponStringsPair> _weaponAnimationsPair;
        
        public Dictionary<Type, WeaponAnimationsConfig> WeaponAnimations 
        {
            get 
            {
                Dictionary<Type, WeaponAnimationsConfig> dictionary = new();
                
                foreach (var pair in _weaponAnimationsPair)
                {
                    if (!dictionary.ContainsKey(pair.Weapon.GetType()))
                    {
                        dictionary.Add(pair.Weapon.GetType(), pair.AnimationsConfig);
                        Debug.Log(pair.Weapon.GetType());
                    }

                }
                
                return dictionary;
            }
        }
    }
}

