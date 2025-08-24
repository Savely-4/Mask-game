using System.Collections;
using Runtime.Entities.Effects;
using Runtime.Entities.Player;
using UnityEngine;

namespace Runtime.Entities.Weapons
{
    public class Weapon_Sword1 : BaseWeapon
    {
        private MeleeStrikeComponent _meleeStrike;

        [SerializeField] private int _baseDamage;
        [SerializeField] private int _maxCombo;

        [Header("Cooldowns")]
        [SerializeField] private float _betweenAttacksCooldown;
        [SerializeField] private float _comboResetCooldown;

        [SerializeField] private MeleeSwipeData[] meleeSwipeDatas;

        private int comboValue;
        private bool canAct = true;

        Coroutine cooldownRoutine;


        //Update: if not attacking and x time passed - reset combo
        void Update()
        {
            if (cooldownRoutine != null)
                return;

            if (canAct && comboValue > 0)
                cooldownRoutine = StartCoroutine(ResetCombo(0.5f));
        }


        public override void Initialize(GameObject playerObject)
        {
            base.Initialize(playerObject);

            //Melee Strike
            _meleeStrike = playerObject.GetComponentInChildren<MeleeStrikeComponent>();

            AnimationControl.MeleeStartedHitting += OnMeleeStartedHitting;
            _meleeStrike.ObjectHit += OnObjectHit;
        }

        void OnEnable()
        {
            if (AnimationControl == null)
                return;

            AnimationControl.MeleeStartedHitting += OnMeleeStartedHitting;
            _meleeStrike.ObjectHit += OnObjectHit;
        }

        void OnDisable()
        {
            _meleeStrike.ObjectHit -= OnObjectHit;
            AnimationControl.MeleeStartedHitting -= OnMeleeStartedHitting;
        }


        public override void OnPrimaryPressed()
        {
            //If able to attack - 
            if (!AnimationControl.IsIdle || !canAct)
                return;

            StartCoroutine(PerformAttack(comboValue));

            //Reset combo cooldown timer
            if (cooldownRoutine != null)
            {
                StopCoroutine(cooldownRoutine);
                cooldownRoutine = null;
            }
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



        private void OnMeleeStartedHitting()
        {
            var swipeData = meleeSwipeDatas[comboValue];
            _meleeStrike.PerformSwipe(swipeData.AngleHorizontalOffset, swipeData.AngleWidth, swipeData.Duration);
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

            comboValue++;

            if (comboValue >= _maxCombo)
                comboValue = 0;

            canAct = true;
        }

        private IEnumerator ResetCombo(float timeToWait)
        {
            yield return new WaitForSeconds(timeToWait);

            comboValue = 0;
        }


        private void OnObjectHit(GameObject gameObject)
        {
            if (!gameObject.TryGetComponent<INormalDamageAble>(out var normalDamageAble))
                return;

            //TODO: Add crits and other on hit effects here
            normalDamageAble.Receive(_baseDamage);
        }
    }
}