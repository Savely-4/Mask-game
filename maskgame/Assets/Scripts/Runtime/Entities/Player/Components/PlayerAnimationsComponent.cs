using System;
using UnityEngine;

namespace Runtime.Entities.Player
{
    public class PlayerAnimationsComponent : MonoBehaviour, IPlayerAnimationsWeaponControl, IPlayerAnimationsMovementControl
    {
        [SerializeField] private Animator _movement;
        [SerializeField] private Animator _weapons;

        [SerializeField] private AnimationsEventsComponent _animationsEvents;

        public event Action MeleeStartedHitting;
        public event Action MeleeStoppedHitting;

        public bool IsIdle { get; private set; } = false;


        void Awake()
        {
            _animationsEvents.ActionStarted += OnActionStarted;
            _animationsEvents.ActionEnded += OnActionEnded;

            _animationsEvents.MeleeStartedHitting += OnMeleeStartedHitting;
            _animationsEvents.MeleeStoppedHitting += OnMeleeStoppedHitting;
        }

        void Start()
        {
            IsIdle = true;
        }

        void OnDestroy()
        {
            _animationsEvents.ActionStarted -= OnActionStarted;
            _animationsEvents.ActionEnded -= OnActionEnded;

            _animationsEvents.MeleeStartedHitting -= OnMeleeStartedHitting;
            _animationsEvents.MeleeStoppedHitting -= OnMeleeStoppedHitting;
        }


        public void SetPrimaryPressed(bool value)
        {
            _weapons.SetBool("Primary", value);
        }

        public void SetSecondaryPressed(bool value)
        {

        }


        public void SetInt(string name, int value)
        {

        }

        //TODO: Add lerp for movement values to make blending smooth
        public void SetRelativeSpeed(float forward, float strafe)
        {
            _movement.SetFloat("Forward", forward);
            _movement.SetFloat("Strafe", strafe);
        }

        public void ToggleAirborne(bool value)
        {
            _movement.SetBool("Airborne", value);
        }

        public void TriggerJump()
        {
            _movement.SetTrigger("Jump");
        }



        private void OnActionStarted()
        {
            IsIdle = false;
            Debug.Log("Action Started");
        }

        private void OnActionEnded()
        {
            IsIdle = true;
            Debug.Log("Action Ended");
        }

        private void OnMeleeStartedHitting()
        {
            MeleeStartedHitting?.Invoke();
        }

        private void OnMeleeStoppedHitting()
        {
            MeleeStoppedHitting?.Invoke();
        }
    }
}