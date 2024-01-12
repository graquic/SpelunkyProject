using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Bat : Enemy
{
    public enum BatState
    {
        Idle,
        Chase,
    }

    BatState curState;
    Animator animator;

    [SerializeField] float moveSpeed;
    [SerializeField] float attackDelay;

    bool canAttack;



    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        canAttack = true;
    }

    protected override void Update()
    {
        ModifyDirection();

        switch(curState)
        {
            case BatState.Idle:
                CheckPlayerInRange();
                break;
            case BatState.Chase:
                ChasePlayer();
                break;
        }
    }

    void CheckPlayerInRange()
    {
        Vector2 playerPos = GameManager.Instance.player.transform.position;

        float dist = Vector2.Distance(playerPos, transform.position);

        if(dist <= detectRange)
        {
            curState = BatState.Chase;
            animator.Play("Chase");
        }
    }

    void ChasePlayer()
    {
        Vector3 playerPos = GameManager.Instance.player.transform.position;
        Vector2 dir = (playerPos - transform.position).normalized;

        rb.velocity = dir * moveSpeed;
    }

    public override void Attack(Player player)
    {
        print(1);
        canAttack = false;
        player.TakeDamage(damage, transform.position);
        StartCoroutine(CheckAttackDelay());
    }

    IEnumerator CheckAttackDelay()
    {
        yield return new WaitForSeconds(attackDelay);

        canAttack = true;
    }

    public override void TakeDamage(int dmg)
    {
        hp -= dmg;

        if (hp <= 0)
        {
            ObjectPoolManager.Instance.ReturnObject(PoolType.Bat, gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player" && canAttack == true)
        {
            Player player = collision.collider.GetComponent<Player>();
            Attack(player);
        }
    }
}
