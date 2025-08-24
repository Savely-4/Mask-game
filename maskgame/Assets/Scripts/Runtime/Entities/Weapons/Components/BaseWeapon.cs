using Runtime.Entities.Player;
using Runtime.Entities.Weapons;
using UnityEngine;

namespace Runtime.Entities.Weapons
{
    public abstract class BaseWeapon : MonoBehaviour, IWeaponInput
    {
        protected IPlayerAnimationsWeaponControl AnimationControl { get; private set; }


        public virtual void Initialize(GameObject playerObject)
        {
            AnimationControl = playerObject.GetComponent<IPlayerAnimationsWeaponControl>();
        }


        public abstract void OnPrimaryPressed();
        public abstract void OnPrimaryReleased();

        public abstract void OnSecondaryPressed();
        public abstract void OnSecondaryReleased();
    }
}