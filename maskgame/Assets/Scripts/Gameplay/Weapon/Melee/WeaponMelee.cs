using System.Collections;
using System.Linq;
using UnityEngine;

public enum MeleeWeaponAttackType 
{
    Raycast,
    Collision
}
public abstract class WeaponMelee : Weapon
{
    [SerializeField] private float _animationElapsedTimePoint;

    [field: SerializeField] public int NumberCollisions { get; set; } = 3;
    [field: SerializeField] public MeleeWeaponAttackType MeleeWeaponAttackType { get; private set; }
    protected sealed override void Attack()
    {
        switch (MeleeWeaponAttackType) 
        {
            case MeleeWeaponAttackType.Raycast:
                    RaycastAttack();
                break;
                
            case MeleeWeaponAttackType.Collision:
                break;
        }
    }
    
    protected void RaycastAttack() 
    {
        Animator.SetTrigger(AttackAnimationName);
        
        StartCoroutine(DelayedAttack());
 
    }
    private IEnumerator DelayedAttack()
    {
        yield return new WaitForSeconds(_animationElapsedTimePoint);

        PerformRaycastAttack();
    }

    protected virtual void PerformRaycastAttack()
    {
        float sphereSize = 1f;
        
        Vector3 center = transform.position + transform.forward * (transform.localScale.z / 2f);
        
        Collider[] hitTargets = Physics.OverlapSphere(center, sphereSize)
            .Take(NumberCollisions)
            .ToArray();
            
        foreach (Collider target in hitTargets)
        {
            OnHitTarget(target);
        }
    }
    
    protected virtual void CollisionAttack() 
    {
        
    }
    
    /*private float GetAnimationLength(string stateName)
    {
        RuntimeAnimatorController ac = Animator.runtimeAnimatorController;
        foreach (var clip in ac.animationClips)
        {
            if (clip.name == stateName)
            return clip.length;
        }

        return 0.3f; // fallback
    }/*/
}
