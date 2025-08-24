using System;
using System.Collections;
using Runtime.Entities.Weapons;
using UnityEngine;

namespace Runtime.Entities.Player
{
    /// <summary>
    /// Class that performs melee strikes from First Person, ensuring consistent hit detection
    /// </summary>
    public class MeleeStrikeComponent : MonoBehaviour
    {
        [SerializeField] private WeaponTriggerComponent swipeObject;

        private Coroutine strikeRoutine;

        public event Action<GameObject> ObjectHit;


        //Angle of swipe relative to horizontal?
        //Delays in returning results?
        //Shoot projectile instead?
        //Fix rig already?

        void Awake()
        {
            swipeObject.ToggleOn(false);
            swipeObject.Collided += OnObjectHit;
        }


        /// <summary>
        /// Perform wide horizontal attack
        /// </summary>
        public void PerformSwipe(float angleOffset, float angleWidth, float duration)
        {
            //Angle and enable swipe trigger component
            //Everything is stored in queue, each object is added only once
            //How to sort by swipe direction? When to call hit event?
            Stop();

            strikeRoutine = StartCoroutine(SwipeRoutine(swipeObject, angleOffset, angleWidth, duration));
        }

        public void PerformStab()
        {

        }

        public void Stop()
        {
            if (strikeRoutine == null)
                return;

            StopCoroutine(strikeRoutine);
            strikeRoutine = null;
        }



        private void OnObjectHit(GameObject hitObject)
        {
            //Send to data structure
            ObjectHit?.Invoke(hitObject);
        }


        IEnumerator SwipeRoutine(WeaponTriggerComponent swipeTrigger, float angleHorizontalOffset, float angleWidth, float duration)
        {
            Debug.LogWarning("Starting Swipe Routine");

            var swipeTransform = swipeTrigger.transform;

            swipeTransform.localPosition = Vector3.zero;
            swipeTransform.localRotation = Quaternion.identity * Quaternion.Euler(0, 0, -angleHorizontalOffset) * Quaternion.Euler(0, -angleWidth, 0);

            //Enable
            swipeTrigger.ToggleOn(true);

            float currentTime = 0;

            while (currentTime <= duration)
            {
                var currentAngle = -angleWidth + 2 * angleWidth * currentTime / duration;
                swipeTransform.localRotation = Quaternion.identity * Quaternion.Euler(0, 0, -angleHorizontalOffset) * Quaternion.Euler(0, currentAngle, 0);

                currentTime += Time.deltaTime;
                yield return null;
            }

            //Disable
            swipeTrigger.ToggleOn(false);

            Debug.LogWarning("Swipe Routine finished");
        }
    }
}