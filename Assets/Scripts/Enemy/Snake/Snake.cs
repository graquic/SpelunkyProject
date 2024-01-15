using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : Enemy
{ 
    private enum SnakeState
    {
        Move, Attack
    }

    SnakeState curState;
    Player player;
    Animator animator;

    [SerializeField] float moveSpeed;
    bool canAttack;

    protected override void Awake()
    {
        base.Awake();

        curState = SnakeState.Move;
        animator = GetComponent<Animator>();
        canAttack = true;
    }

    private void Start()
    {
        player = GameManager.Instance.player;
    }

    private void OnEnable()
    {
        canAttack = true;
    }

    private void OnDisable()
    {
        canAttack = false;
    }
    protected override void Update()
    {
        float dist = Vector2.Distance(transform.position, player.transform.position);

        switch (curState)
        {
            case SnakeState.Move:
                if (dist < detectRange && CheckFrontPlayer(player))
                { curState = SnakeState.Attack;}

                rb.velocity = new Vector2(transform.localScale.x * moveSpeed, rb.velocity.y);
                break;
            case SnakeState.Attack:
                int attDir = (int) Mathf.Sign(player.transform.position.x - transform.position.x);
                transform.localScale = new Vector3(attDir, 1, 1);

                Attack(player);

                if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    canAttack = true;
                    
                    curState = SnakeState.Move;
                }
                break;
        }

        animator.SetInteger("State", (int)curState);
    }
    

    bool CheckFrontPlayer(Player player)
    {
        float diffX = transform.position.x - player.transform.position.x;

        if(diffX > 0 && transform.localScale.x < 0)
        {
            return true;
        }
        else if(diffX < 0 && transform.localScale.x > 0)
        {
            return true;
        }

        return false;
    }

    
    public override void Attack(Player player)
    {
        if(canAttack == true)
        {
            canAttack = false;
            player.TakeDamage(damage, transform.position);
        }
    }

    public override void TakeDamage(int dmg)
    {
        hp -= dmg;

        if( hp <= 0)
        {
            ObjectPoolManager.Instance.ReturnObject(PoolType.Snake, gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Ground")
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player")
        {
            Player player2 = collision.collider.GetComponent<Player>();
            Attack(player2);
        }
    }


}
