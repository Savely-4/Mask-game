using System.Collections;
using Runtime.Entities.Effects;
using Runtime.Entities.Player;
using UnityEngine;

namespace Runtime.Entities.Weapons
{
    public class Weapon_Sword : BaseWeapon
    {
        [SerializeField] private WeaponTriggerComponent _weaponTrigger;

        [SerializeField] private int _baseDamage;
        [SerializeField] private int _maxCombo;

        [Header("Cooldowns")]
        [SerializeField] private float _betweenAttacksCooldown;
        [SerializeField] private float _comboResetCooldown;

        private int comboValue;
        private bool canAct = true;

        Coroutine cooldownRoutine;


        void Awake()
        {
            _weaponTrigger.ToggleOn(false);

            _weaponTrigger.Collided += OnObjectHit;
        }

        //Update: if not attacking and x time passed - reset combo
        void Update()
        {
            if (cooldownRoutine != null)
                return;

            if (canAct && comboValue > 0)
                cooldownRoutine = StartCoroutine(ResetCombo(0.5f));
        }


        public override void Initialize(IPlayerAnimationsWeaponControl animationControl)
        {
            base.Initialize(animationControl);

            AnimationControl.MeleeStartedHitting += OnMeleeStartedHitting;
            AnimationControl.MeleeStoppedHitting += OnMeleeStoppedHitting;
        }

        void OnEnable()
        {
            if (AnimationControl == null)
                return;

            AnimationControl.MeleeStartedHitting += OnMeleeStartedHitting;
            AnimationControl.MeleeStoppedHitting += OnMeleeStoppedHitting;
        }

        void OnDisable()
        {
            AnimationControl.MeleeStartedHitting -= OnMeleeStartedHitting;
            AnimationControl.MeleeStoppedHitting -= OnMeleeStoppedHitting;
        }


        public override void OnPrimaryPressed()
        {
            //If able to attack - 
            if (!AnimationControl.IsIdle || !canAct)
                return;

            StartCoroutine(PerformAttack(comboValue));

            if (cooldownRoutine != null)
            {
                StopCoroutine(cooldownRoutine);
                cooldownRoutine = null;
            }

            comboValue++;

            if (comboValue >= _maxCombo)
                comboValue = 0;
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

            yield return new WaitForSeconds(_betweenAttacksCooldown);

            Debug.Log($"Attack finished, isIdle {AnimationControl.IsIdle}");

            canAct = true;
        }

        private IEnumerator ResetCombo(float timeToWait)
        {
            yield return new WaitForSeconds(timeToWait);

            comboValue = 0;
        }


        private void OnMeleeStartedHitting() => _weaponTrigger.ToggleOn(true);
        private void OnMeleeStoppedHitting() => _weaponTrigger.ToggleOn(false);

        private void OnObjectHit(GameObject gameObject)
        {
            if (!gameObject.TryGetComponent<INormalDamageAble>(out var normalDamageAble))
                return;

            //TODO: Add crits and other on hit effects here
            normalDamageAble.Receive(_baseDamage);
        }
    }
}