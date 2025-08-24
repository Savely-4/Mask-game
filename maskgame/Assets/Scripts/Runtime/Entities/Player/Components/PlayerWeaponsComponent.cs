using Runtime.Entities.Weapons;
using UnityEngine;

namespace Runtime.Entities.Player
{
    //TODO: Weapon Switching
    public class PlayerWeaponsComponent : MonoBehaviour, IPlayerWeaponsInput
    {
        [SerializeField] private BaseWeapon weapon;
        [SerializeField] private PlayerAnimationsComponent animationsComponent;


        void Awake()
        {
            weapon.Initialize(gameObject);
        }


        //Handle switching weapons, weapons input, animations and effects
        public void SetPrimaryPressed()
        {
            weapon.OnPrimaryPressed();
        }

        public void SetPrimaryReleased()
        {
            weapon.OnPrimaryReleased();
        }


        public void SetSecondaryPressed()
        {
            weapon.OnSecondaryPressed();
        }

        public void SetSecondaryReleased()
        {
            weapon.OnSecondaryReleased();
        }
    }
}