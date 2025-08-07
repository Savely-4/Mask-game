using UnityEngine;

namespace Runtime.Entities.Weapons
{
    [System.Serializable]
    public abstract class BaseEffect : IUnitEffect
    {
        public abstract void Apply(GameObject gameObject);
    }
}