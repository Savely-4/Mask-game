using System;
using UnityEngine;

namespace WeaponSystem
{
    public abstract class Weapon : MonoBehaviour
    {
        [field: SerializeField] public float Damage { get; set; }
        [field: SerializeField] public float AttackRate { get; set; }
        [SerializeField] protected LayerMask HittableLayers;
        public Transform AttackPoint { get; private set; }
        private float _lastAttackTime = Mathf.NegativeInfinity;
        public event Action OnAttack;
        public event Action<Collision[]> OnHits;
    
        public void TryAttack(Transform attackPoint) 
        {
            if (CanAttack()) 
            {
                AttackPoint = attackPoint;
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
            return _lastAttackTime <= Time.time + 1f / AttackRate;
        }
    
    }
}
