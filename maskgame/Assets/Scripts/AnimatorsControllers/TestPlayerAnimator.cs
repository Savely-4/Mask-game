using UnityEngine;

public class TestPlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private WeaponAnimatorConfig _weaponAnimatorConfig;


    public WeaponAnimator WeaponAnimator { get; private set; }
    
    private void Awake()
    {
        WeaponAnimator = new(_weaponAnimatorConfig, _animator);
    }
    
}
