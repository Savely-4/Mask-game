using System;
using Runtime.Entities.WeaponSystem;
using Runtime.Interfaces.WeaponSystem.Melee;

namespace Runtime.Services.CombatSystem
{
    public class WeaponCombatModel
    {
        private Weapon _currentWeapon;

        public Weapon CurrentWeapon
        {
            get => _currentWeapon;

            set
            {
                OnBeforeChangingCurrentWeapon?.Invoke(_currentWeapon);
                _currentWeapon = value;
                OnAfterChangingCurrentWeapon?.Invoke(_currentWeapon);
            }
        }

        public Action<Weapon> OnBeforeChangingCurrentWeapon;
        public Action<Weapon> OnAfterChangingCurrentWeapon;

        public void MainAttackUpdate()
        {
            if (_currentWeapon != null)
            {
                _currentWeapon.TryAttack();
            }
        }

        public void AlternativeAttackUpdate()
        {
            if (_currentWeapon != null)
            {
                if (_currentWeapon is IAlternateAttackable alternateAttackable)
                {
                    alternateAttackable.AlternateAttack();
                }
            }
        }
    }

}
