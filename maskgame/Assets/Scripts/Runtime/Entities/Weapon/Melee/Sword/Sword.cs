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
            
            byte r = (byte)Random.Range(0, 255);
            byte g = (byte)Random.Range(0, 255);
            byte b = (byte)Random.Range(0, 255);
            
            Color32 randColor = new(r, g, b, 1);

            hitTarget.Collider.gameObject.GetComponent<MeshRenderer>().material.color = randColor;
        }
        
    }
}
