using System;
using UnityEngine;

namespace Runtime.Entities.WeaponSystem
{
    public abstract class Weapon : MonoBehaviour
    {
        [field: SerializeField] public float Damage { get; set; }
        [field: SerializeField] public float AttackRate { get; set; }
        [SerializeField] protected LayerMask HittableLayers;
        [field: SerializeField] public Transform AttackPoint { get; private set; }
        private float _lastAttackTime = Mathf.NegativeInfinity;
        public event Action OnAttack;
        public event Action<Collision[]> OnHits;
    
        public void TryAttack() 
        {
            if (CanAttack()) 
            {
                PerformAttack();
                
                _lastAttackTime = Time.time;
                OnAttack?.Invoke();
                Debug.Log("Attack!");
            }
        }
    
        protected virtual void OnHitTargets(params Collision[] targets) 
        {
            Debug.Log("Вы попали в целей");
            OnHits?.Invoke(targets);
        }
        
        /// <summary>
        /// Attack logic method (implementation)
        /// </summary>
        protected abstract void PerformAttack();
 
        protected virtual bool CanAttack() 
        {
            return HasTimePassed();
        }
    
        private bool HasTimePassed() 
        {
            return Time.time >= _lastAttackTime + 1f / AttackRate;
        }
    }
}
