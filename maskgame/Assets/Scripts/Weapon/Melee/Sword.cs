using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    Animator animator;

    [SerializeField] private float damage;
    [SerializeField] private Vector3 boxHalf = new Vector3(1.5f, 1f, 1f);
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] bool isBurnDmg = false;
    [SerializeField] private float burnDuration = 4f;

    Transform player;
    Transform attackPoint;
    private void Start()
    {
        animator = GetComponent<Animator>();
        player = Player.player.hands;
        Player.player.SwordInHands = true;
        gameObject.transform.SetParent(player);
        transform.position = Vector3.zero;
        attackPoint = transform;
    }
    public override void Attack()
    {
        animator.Play($"{Random.Range(1,3)}");//рандомная анимация удара меча!
        Collider[] enemyInBox = Physics.OverlapBox(attackPoint.position, boxHalf, attackPoint.rotation, enemyLayer);
        foreach(Collider enemy in enemyInBox)
        {
            enemy.GetComponent<HpOnObject>().ChangeHp(damage,burnDuration, isBurnDmg);
        }
    }
    
}
