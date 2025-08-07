using Runtime.Entities.Effects;
using UnityEngine;

namespace Runtime.Entities.Player
{
    public class PlayerHealthComponent : MonoBehaviour, INormalDamageAble
    {
        public int health;


        public void Receive(int damage)
        {
            health -= damage;
        }
    }
}