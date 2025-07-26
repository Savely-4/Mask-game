using System.Collections;
using UnityEngine;

namespace Runtime.Entities.Weapons
{
    public class Weapon_Sword : BaseWeapon
    {
        private bool canAttack = true;


        public override void OnPrimaryPressed()
        {
            //If able to attack - 
        }

        public override void OnPrimaryReleased()
        {

        }

        public override void OnSecondaryPressed()
        {

        }

        public override void OnSecondaryReleased()
        {

        }



        private IEnumerator PerformAttack(int combo)
        {
            canAttack = false;

            AnimationControl.SetInt("Combo", 0);
            AnimationControl.SetPrimaryPressed(true);

            yield return null;
            yield return new WaitUntil(() => AnimationControl.IsIdle);

            AnimationControl.SetPrimaryPressed(false);

            canAttack = true;
        }
    }
}