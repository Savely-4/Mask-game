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
            Instantiate(_flashEffect, WeaponModel.AttackPoint.position, Quaternion.identity);
        }
    
        private void HitEffectPlay(Collision[] targets) 
        {
            for (int i = 0; i < targets.Length; i++) 
            {
                Vector3 normal = targets[i].contacts[0].normal;
                Vector3 position = targets[i].contacts[0].point;
            
                Instantiate(_hitEffect, position, Quaternion.LookRotation(normal));
            }
        
        }
    }
}
