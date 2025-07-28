using System.Collections;
using UnityEngine;

namespace Runtime.Entities.Weapons
{
    public class Weapon_Sword : BaseWeapon
    {
        [SerializeField] private float attackCooldown;

        private bool canAct = true;


        public override void OnPrimaryPressed()
        {
            //If able to attack - 
            if (!AnimationControl.IsIdle || !canAct)
                return;

            StartCoroutine(PerformAttack(0));
        }

        public override void OnPrimaryReleased()
        {
            AnimationControl.SetPrimaryPressed(false);
        }

        public override void OnSecondaryPressed()
        {

        }

        public override void OnSecondaryReleased()
        {

        }



        private IEnumerator PerformAttack(int combo)
        {
            canAct = false;

            AnimationControl.SetInt("Combo", combo);
            AnimationControl.SetPrimaryPressed(true);

            Debug.Log($"Attack started, isIdle {AnimationControl.IsIdle}");

            yield return null;

            AnimationControl.SetPrimaryPressed(false);
            Debug.Log($"Frame later, isIdle {AnimationControl.IsIdle}");

            yield return null;
            yield return new WaitWhile(() => !AnimationControl.IsIdle);

            Debug.Log($"IsIdle is true, waiting for cooldown, IsIdle {AnimationControl.IsIdle}");

            yield return new WaitForSeconds(attackCooldown);

            Debug.Log($"Attack finished, isIdle {AnimationControl.IsIdle}");

            canAct = true;
        }
    }
}