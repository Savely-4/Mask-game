using System;
using UnityEngine;

namespace Runtime.Entities.Player
{
    public class PlayerAnimationsComponent : MonoBehaviour, IPlayerAnimationsWeaponControl, IPlayerAnimationsMovementControl
    {
        [SerializeField] private Animator movement;
        [SerializeField] private Animator weapons;

        public bool IsIdle => throw new NotImplementedException();


        public void SetPrimaryPressed(bool value)
        {

        }

        public void SetSecondaryPressed(bool value)
        {

        }


        public void SetInt(string name, int value)
        {

        }


        public void SetRelativeSpeed(float forward, float Strafe)
        {
            movement.SetFloat("Forward", forward);
            movement.SetFloat("Strafe", Strafe);
        }

        public void ToggleAirborne(bool value)
        {
            movement.SetBool("Airborne", value);
        }

        public void TriggerJump()
        {
            movement.SetTrigger("Jump");
        }
    }
}