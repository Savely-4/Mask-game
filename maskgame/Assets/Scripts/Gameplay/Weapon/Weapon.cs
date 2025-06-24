using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected Transform attackPoint;

    public abstract void Attack();
    public virtual void Equip(Transform hand)
    {
        transform.SetParent(hand);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        attackPoint = transform;
    }
}
