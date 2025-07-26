using System;
using UnityEngine;

namespace Runtime.Services.CombatSystem
{
    public class WeaponCombatView : IDisposable
    {
        private readonly Animator _animator;

        public WeaponCombatView(Animator animator)
        {
            _animator = animator;
        }
        public void Dispose()
        {

        }

        public void MainAttackAnimationPlay(string[] attackAnimationsName)
        {
            int randIndex = UnityEngine.Random.Range(0, attackAnimationsName.Length);

            Debug.Log(attackAnimationsName[randIndex]);
            _animator.SetTrigger(attackAnimationsName[randIndex]);

        }
    }
}
