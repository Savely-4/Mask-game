using System;
using UnityEngine;
using WeaponSystem;
public class WeaponAnimator : IDisposable
{
    private readonly Animator _animator;
    private readonly WeaponAnimatorConfig _weaponAnimatorConfig;
    private WeaponCombatSystem _weaponCombatSystem;
    
    public WeaponCombatSystem WeaponCombatSystem  
    {
        get => _weaponCombatSystem;
        
        set 
        {
            if (_weaponCombatSystem != null) 
            {
                _weaponCombatSystem.OnBeforeChangingCurrentWeapon -= OnBeforeChangingCurrentWeapon;
                _weaponCombatSystem.OnCurrentWeaponChanged -= OnCurrentWeaponChanged;
            }
            
            _weaponCombatSystem = value;
            
            _weaponCombatSystem.OnBeforeChangingCurrentWeapon += OnBeforeChangingCurrentWeapon;
            _weaponCombatSystem.OnCurrentWeaponChanged += OnCurrentWeaponChanged;
        }
    }
    
    public WeaponAnimator(WeaponAnimatorConfig weaponAnimatorConfig, Animator animator) 
    {
        _weaponAnimatorConfig = weaponAnimatorConfig;
        _animator = animator;
    }
    public void Dispose()
    {
        if (_weaponCombatSystem != null) 
        {
            _weaponCombatSystem.OnBeforeChangingCurrentWeapon -= OnBeforeChangingCurrentWeapon;
            _weaponCombatSystem.OnCurrentWeaponChanged -= OnCurrentWeaponChanged;
        }
    }
    private void OnBeforeChangingCurrentWeapon(Weapon weapon) 
    {
        if (weapon != null) 
        {
            weapon.OnAttack -= MainAttackAnimationPlay;
        }
    }
    
    private void OnCurrentWeaponChanged(Weapon weapon) 
    {
        if (weapon != null) 
        {
            weapon.OnAttack += MainAttackAnimationPlay;
        }
    }
    
    private void MainAttackAnimationPlay()
    {
        if (_weaponAnimatorConfig.WeaponAnimations.TryGetValue(_weaponCombatSystem.CurrentWeapon, out var weaponAnimationsConfig))
        {
            string[] attackAnimationsName = weaponAnimationsConfig.MainAttackAnimations.ToArray();
            int randIndex = UnityEngine.Random.Range(0, attackAnimationsName.Length);
            
            _animator.SetTrigger(attackAnimationsName[randIndex]);
        }
    }
}
