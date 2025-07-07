using WeaponSystem.Melee;

namespace Runtime.Entities.WeaponSystem
{
    public class WeaponCombatSystem
    {
        private Weapon _currentWeapon;

        public Weapon CurrentWeapon
        {
            get => _currentWeapon;

            set
            {
                _currentWeapon = value;
            }
        }
        
        public void MainAttackUpdate(bool mainAttackInput)
        {
            if (_currentWeapon != null && mainAttackInput)
            {
                _currentWeapon.TryAttack();
            }
        }

        public void AlternativeAttackUpdate(bool alternativeAttackInput)
        {
            if (_currentWeapon != null && alternativeAttackInput)
            {
                if (_currentWeapon is IAlternateAttackable alternateAttackable)
                {
                    alternateAttackable.AlternateAttack();
                }
            }
        }
    }
}
