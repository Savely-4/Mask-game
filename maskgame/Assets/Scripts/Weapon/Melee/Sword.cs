using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    Animator animator;//вынести

<<<<<<< Updated upstream
    [SerializeField] private float damage;
    [SerializeField] private Vector3 boxHalf;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] bool isBurnDmg = false;
    [SerializeField] private float burnDuration = 4f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        Equip(Player.player.hands);
        Player.player.SwordInHands = true;
        transform.position = new Vector3(0, 1.3f, 1.2f);
    }
    public override void Attack()
    {
        animator?.Play($"{Random.Range(1,3)}");//рандомная анимация удара меча!
=======
    [SerializeField] private float damage = 30;//вынести
    [SerializeField] private Vector3 boxHalf = new Vector3(1.5f, 1f, 1f);
    [SerializeField] private LayerMask enemyLayer;//мб вынести
    [SerializeField] bool isBurnDmg = false;//вынести
    [SerializeField] private float burnDuration = 4f;//вынести

    Transform player;//вынести
    Transform attackPoint;//вынести
    private void Start()
    {
        animator = GetComponent<Animator>();
        //player = Player.player.hands;
       // gameObject.transform.SetParent(player);
        transform.position = Vector3.zero;
        attackPoint = transform;
    }
    public override void Attack()
    {
        //animator.Play($"{Random.Range(1,3)}");//рандомная анимация
        Debug.Log(1);
>>>>>>> Stashed changes
        Collider[] enemyInBox = Physics.OverlapBox(attackPoint.position, boxHalf, attackPoint.rotation, enemyLayer);
        foreach(Collider enemy in enemyInBox)
        {
            enemy.GetComponent<HpOnObject>().ChangeHp(damage,burnDuration, isBurnDmg);
            Debug.Log(enemy.GetComponent<HpOnObject>().hp);
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            attackPoint = transform;

        Gizmos.color = Color.red;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(attackPoint.position, attackPoint.rotation, Vector3.one);
        Gizmos.matrix = rotationMatrix;
        Gizmos.DrawWireCube(Vector3.zero, boxHalf * 2f);
    }
#endif
}
