using UnityEngine;

namespace Runtime.Entities.Weapons
{
    public interface IUnitEffect
    {
        void Apply(GameObject unit);
    }
}