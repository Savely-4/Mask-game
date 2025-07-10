using System;
using Runtime.Entities.WeaponSystem;
using UnityEngine;

namespace Runtime.Services.CombatSystem
{
    public class WeaponCombatPresenter : IDisposable
    {
        private readonly WeaponCombatPresenterConfig _config;
        
        private readonly WeaponCombatModel _weaponCombatModel;
        private readonly WeaponCombatView _weaponCombatView;

        public WeaponCombatPresenter(WeaponCombatPresenterConfig config, Animator animator)
        {
            _config = config;
            
            _weaponCombatModel = new();
            _weaponCombatView = new(animator);


            _weaponCombatModel.OnBeforeChangingCurrentWeapon += OnBeforeChangingCurrentWeapon;
            _weaponCombatModel.OnAfterChangingCurrentWeapon += OnAfterChangingCurrentWeapon;
        }

        public void Dispose()
        {
            if (_weaponCombatModel == null) return;
            
            _weaponCombatModel.OnBeforeChangingCurrentWeapon -= OnBeforeChangingCurrentWeapon;
            _weaponCombatModel.OnAfterChangingCurrentWeapon -= OnAfterChangingCurrentWeapon;
        }

        public void SetNewWeapon(Weapon weapon) 
        {
            _weaponCombatModel.CurrentWeapon = weapon;
        }
        private void OnBeforeChangingCurrentWeapon(Weapon weapon) 
        {
            if (weapon != null) 
            {
                weapon.OnAttack -= OnAttack;
            }
        }
    
        private void OnAfterChangingCurrentWeapon(Weapon weapon) 
        {
            if (weapon != null) 
            {
                weapon.OnAttack += OnAttack;
            }
        }
        
        private void OnAttack() 
        {
            var weaponAnimations = _config.WeaponAnimations;
        
            if (weaponAnimations.TryGetValue(_weaponCombatModel.CurrentWeapon, out var animationsConfig)) 
            {
                _weaponCombatView.MainAttackAnimationPlay(animationsConfig.MainAttackAnimations.ToArray());
            }
        }
        
    }
}
