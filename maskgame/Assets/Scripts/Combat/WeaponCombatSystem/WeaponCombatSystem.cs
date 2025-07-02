using System;
public class WeaponCombatSystem
{
    private Weapon _currentWeapon;
    public Weapon CurrentWeapon 
    {
        get => _currentWeapon;
        
        set 
        {
            OnBeforeChangingCurrentWeapon?.Invoke(_currentWeapon);
            _currentWeapon = value;
            OnCurrentWeaponChanged?.Invoke(_currentWeapon);
        }
    }

    public event Action<Weapon> OnCurrentWeaponChanged;
    public event Action<Weapon> OnBeforeChangingCurrentWeapon;
    
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
