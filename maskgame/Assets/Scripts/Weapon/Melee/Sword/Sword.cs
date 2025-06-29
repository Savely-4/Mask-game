using UnityEngine;

public class Sword : WeaponMelee, IAlternateAttackable
{
    public void AlternateAttack()
    {
        Debug.Log("Альтернативная атака");
    }
}
