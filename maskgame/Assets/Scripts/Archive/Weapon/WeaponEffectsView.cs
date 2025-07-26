using Runtime.Entities.WeaponSystem;
using UnityEngine;

namespace Runtime.Services.WeaponSystem
{
    public class WeaponEffectsView : MonoBehaviour
    {
        [Header("Model")]
        [field: SerializeField] protected Weapon WeaponModel { get; private set; }


        [Header("Effects")]
        [SerializeField] private ParticleSystem _hitEffect;
    
        [SerializeField] private ParticleSystem _flashEffect;
    
        protected virtual void OnEnable()
        {
            WeaponModel.OnAttack += AttackEffectPlay;
            WeaponModel.OnHits += HitEffectPlay;
        }

        protected virtual void OnDisable()
        {
            WeaponModel.OnAttack -= AttackEffectPlay;
            WeaponModel.OnHits -= HitEffectPlay;
        }

        private void AttackEffectPlay() 
        {
            //Instantiate(_flashEffect, WeaponModel.AttackPoint.position, Quaternion.identity);
        }
    
        private void HitEffectPlay(HitTarget3D target) 
        {
            int countContacts = target.Collision.contactCount;
        
            for (int i = 0; i < countContacts; i++) 
            {
                Vector3 normal = target.Collision.contacts[i].normal;
                Vector3 position = target.Collision.contacts[i].point;
   
                var hitObj = Instantiate(_hitEffect, position, Quaternion.LookRotation(normal));
                Destroy(hitObj, 3);
            }
        
        }
    }
}
