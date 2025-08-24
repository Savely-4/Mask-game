using UnityEngine;

namespace Runtime.Entities.Effects
{
    /// <summary>
    /// Object with this interface is able to receive normal damage
    /// </summary>
    public interface INormalDamageAble
    {
        void Receive(int damage);
    }
}