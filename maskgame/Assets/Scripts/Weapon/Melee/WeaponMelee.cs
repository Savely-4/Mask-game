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
    [field: SerializeField] public int NumberCollisions { get; set; } = 3;
    [field: SerializeField] public MeleeWeaponAttackType MeleeWeaponAttackType { get; private set; }

    private float _attackTimePointAnim;

    protected void Start()
    {
        _attackTimePointAnim = GetAnimationLength(AttackAnimationName);
    }
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
        yield return new WaitForSeconds(_attackTimePointAnim * 0.5f);

        PerformRaycastAttack();
    }

    protected virtual void PerformRaycastAttack()
    {
        float sphereSize = 1f;
        
        //Vector3 center = transform.position + transform.forward * (transform.localScale.z / 2f);
        
        Collider[] hitTargets = Physics.OverlapSphere(AttackPoint.position, sphereSize)
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
    
    private float GetAnimationLength(string stateName)
    {
        RuntimeAnimatorController ac = Animator.runtimeAnimatorController;
        foreach (var clip in ac.animationClips)
        {
            if (clip.name == stateName)
            return clip.length;
        }

        return 0.3f; // fallback
    }
}
