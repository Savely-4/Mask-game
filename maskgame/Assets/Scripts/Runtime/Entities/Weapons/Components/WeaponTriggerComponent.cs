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

        private Collider[] _colliders;

        private bool initialized = false;


        void Awake()
        {
            _colliders = GetComponents<Collider>();
            initialized = true;
        }


        public void ToggleOn(bool value)
        {
            enabled = value;

            if (!initialized)
                return;

            foreach (var collider in _colliders)
                collider.enabled = value;
        }


        void OnTriggerEnter(Collider other)
        {
            if (!enabled)
                return;

            Debug.Log($"Collision: {other.gameObject}");

            Collided?.Invoke(other.gameObject);
        }
    }
}