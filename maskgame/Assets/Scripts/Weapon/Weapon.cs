using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [field: SerializeField] protected Transform AttackPoint { get; private set; }
    [field: SerializeField] protected string AttackAnimationName { get; private set; }
    
    [field: SerializeField] public float Damage { get; set; }
    [field: SerializeField] public float AttackRate { get; set; }
    
    private float _lastAttackTime = Mathf.NegativeInfinity;
    
    public Animator Animator { get; set; }
    
    public void TryAttack() 
    {
        if (CanAttack()) 
        {
            Attack();
            
            _lastAttackTime = Time.time;
        }
    }
    
    protected virtual void OnHitTarget(Collider target) 
    {
        Debug.Log("Вы попали в цель");
    }
    protected abstract void Attack();
 
    protected virtual bool CanAttack() 
    {
        return HasTimePassed();
    }
    
    private bool HasTimePassed() 
    {
        return _lastAttackTime <= Time.time + 1f / _lastAttackTime;
    }
    
}
