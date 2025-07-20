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
        private readonly PlayerInputKeyboardService _inputService;

        public WeaponCombatPresenter(WeaponCombatPresenterConfig config, Animator animator, PlayerInputKeyboardService inputService)
        {
            _config = config;
            
            _weaponCombatModel = new();
            _weaponCombatView = new(animator);
            _inputService = inputService;

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

        public void HandleWeaponActions()
        {
            if (_weaponCombatModel.CurrentWeapon == null) return; 

            if(_inputService.PrimaryAttackButtonPressed(true))
            {
                _weaponCombatModel.MainAttackUpdate();
            }

            if (_inputService.AlternateAttackKeyPressed(true))
            {
                _weaponCombatModel.AlternativeAttackUpdate();
            }
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
            Debug.Log(_weaponCombatModel.CurrentWeapon.GetType());
            
            if (weaponAnimations.TryGetValue(_weaponCombatModel.CurrentWeapon.GetType(), out var animationsConfig)) 
            {
                _weaponCombatView.MainAttackAnimationPlay(animationsConfig.MainAttackAnimations.ToArray());
            }

        }
        
    }
}
