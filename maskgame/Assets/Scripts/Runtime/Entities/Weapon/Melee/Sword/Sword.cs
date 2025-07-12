using System.Collections;
using System.Collections.Generic;
using Runtime.Interfaces.WeaponSystem.Melee;
using Runtime.InventorySystem;
using UnityEngine;

namespace Runtime.Entities.WeaponSystem.Melee
{
    public class Sword : WeaponMelee, IAlternateAttackable, IPickableItem
    {
        [field: SerializeField] public ItemData ItemData { get; private set; }

        public void AlternateAttack()
        {
            Debug.Log("Альтернативная атака");
        }

        protected override void OnHitTarget(HitTarget3D hitTarget)
        {
            base.OnHitTarget(hitTarget);
            
            
        }
        
    }
}
