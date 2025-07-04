using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem.Melee
{
    public abstract class WeaponMelee : WeaponSystem.Weapon
    {
        [field: SerializeField] public int NumberCollisions { get; set; } = 3;
        [field: SerializeField] public float DelayAttack { get; set; } = 0.35f; // time, after that start attack
        [field: SerializeField] public float ActiveTimeAttack { get; set; } = 0.5f; // time attack duration

        private bool _activeAttack;
        private Coroutine _delayAttackRoutine;
        private int _currentNumberCollisions;
        private readonly HashSet<Collider> _alreadyHit = new(); //so as not to touch already touched targets

        private void Awake()
        {
            _activeAttack = false;
        }

        protected override void PerformAttack()
        {
            _currentNumberCollisions = 0;
            _alreadyHit.Clear();

            if (_delayAttackRoutine != null)
            {
                StopCoroutine(_delayAttackRoutine);
                _activeAttack = false;
            }

            _delayAttackRoutine = StartCoroutine(DelayAttackRoutine());
        }

        private IEnumerator DelayAttackRoutine()
        {
            yield return new WaitForSeconds(DelayAttack);
            _activeAttack = true;
            yield return new WaitForSeconds(ActiveTimeAttack);
            _activeAttack = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!_activeAttack) return;
    
            if ((HittableLayers.value & (1 << collision.gameObject.layer)) == 0)
                return;
            
            if (_alreadyHit.Contains(collision.collider))
                return;

            _alreadyHit.Add(collision.collider);

            if (_currentNumberCollisions >= NumberCollisions)
                return;

            _currentNumberCollisions++;
            OnHitTargets(collision);
        }
    }
}
