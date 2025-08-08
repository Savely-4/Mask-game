using Runtime.Entities.Effects;
using UnityEngine;

namespace Runtime.Entities.Enemy
{
    public class EnemyHealthComponent : MonoBehaviour, INormalDamageAble
    {
        [SerializeField] private int health;


        public void Receive(int damage)
        {
            health -= damage;
            Debug.Log($"Received {damage} damage, health now {health}");
        }
    }
}