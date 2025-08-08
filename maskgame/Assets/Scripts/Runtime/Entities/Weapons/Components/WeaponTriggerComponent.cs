using System;
using UnityEngine;

namespace Runtime.Entities.Weapons
{
    /// <summary>
    /// Simplifies work with weapon and projectile collision triggers
    /// </summary>
    public class WeaponTriggerComponent : MonoBehaviour
    {
        public event Action<GameObject> Collided;
        //TODO: Add collision queue and other parameters if needed

        [SerializeField] private Collider _collider;


        public void ToggleOn(bool value)
        {
            _collider.enabled = value;
        }


        void OnTriggerEnter(Collider other)
        {
            Debug.Log($"Collision: {other.gameObject}");

            Collided?.Invoke(other.gameObject);
        }
    }
}