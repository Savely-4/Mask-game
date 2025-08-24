using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerItemHolderConfig", menuName = "Scriptable Objects/Player/PlayerItemHolderConfig")]
public class PlayerItemHolderConfig : ScriptableObject
{
    //[SerializeField] private List<LocalOrientationObjects> _localOrientationObjects; // not working yet
    
    [field: SerializeField] public Vector3 HolderPointOffset { get; private set; }
    [field: SerializeField] public Vector3 HolderPointOrientation { get; private set; }
    [field: SerializeField] public string HolderPointName { get; private set; }
    
    /*/public Dictionary<Type, Orientation> LocalOrientationObjects 
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
    }/*/
}
