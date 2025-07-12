using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerItemHolderConfig", menuName = "Scriptable Objects/Player/PlayerItemHolderConfig")]
public class PlayerItemHolderConfig : ScriptableObject
{
    [SerializeField] private List<LocalOrientationObjects> _localOrientationObjects;
    
    public Dictionary<Type, Orientation> LocalOrientationObjects 
    {
        get 
        {
            Dictionary<Type, Orientation> dictionary = new();
                
            foreach (var pair in _localOrientationObjects)
            {
                if (!dictionary.ContainsKey(pair.ObjectType))
                {
                    dictionary.Add(pair.ObjectType, pair.Orientation);
                }
            }
                
            return dictionary;
        }
    }
}
