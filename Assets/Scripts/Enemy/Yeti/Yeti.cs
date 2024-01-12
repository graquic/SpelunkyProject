using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public enum YetiState
{
    YIdle, YMove, YSmash, YStunned, YDeath
}

public class Yeti : Enemy
{
    YetiState curState;
    public YetiState CurState { get { return curState; } }

    Animator animator;
    Player targetPlayer = null;
    public Player TargetPlayer { get { return targetPlayer; } }

    StateBase<Yeti>[] states = new StateBase<Yeti>[Enum.GetValues(typeof(YetiState)).Length];

    [SerializeField] float moveSpeed;
    [SerializeField] int smashPower;
    [SerializeField] TextMeshProUGUI testText;
    
    int moveDir;
    public int MoveDir { get { return moveDir; } }


    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        states[(int)YetiState.YIdle] = new YIdleState(this);
        states[(int)YetiState.YMove] = new YMoveState(this);
        states[(int)YetiState.YSmash] = new YSmashState(this);
        states[(int)YetiState.YStunned] = new YStunnedState(this);
        states[(int)YetiState.YDeath] = new YDeathState(this);

        curState = YetiState.YIdle;
        states[(int)curState].Enter();
    }

    protected override void Update()
    {
        testText.text = curState.ToString();

        base.Update();

        states[(int)curState].Update();

    }

    public void ChangeState(YetiState state)
    {
        states[(int)curState].Exit();
        curState = state;
        states[(int)curState].Enter();
    }

    public void ChangeAnimation(YetiState state)
    {
        animator.Play(state.ToString());
    }

    public void ChangeAnimation(String name)
    {
        animator.Play(name);
    }

    public bool CheckCurrentAnimationEnd()
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
    }

    public void SetMoveDirection()
    {
        moveDir = UnityEngine.Random.Range(0, 2) * 2 - 1;
    }


    public void Move()
    {
        rb.velocity = new Vector2(moveDir * moveSpeed, rb.velocity.y);
    }

    
    public override void TakeDamage(int Dmg)
    {
        hp -= Dmg;
        if(curState != YetiState.YDeath)
        {
            ChangeState(YetiState.YStunned);
        }

        if(hp <= 0)
        {
            ChangeState(YetiState.YDeath);
        }
    }

    public void TakeDamage(int Dmg, Vector3 velocity)
    {
        hp -= Dmg;
        if (curState != YetiState.YDeath)
        {
            ChangeState(YetiState.YStunned);
        }

        if (hp <= 0)
        {
            ChangeState(YetiState.YDeath);
        }
    }

    public override void Attack(Player player)
    {
        Vector3 dir = (player.transform.position + new Vector3(0, 1f, 0) - transform.position).normalized;
        player.Rb.AddForce(dir * smashPower, ForceMode2D.Impulse);

        player.SetIsSmashed(true);
        GameManager.Instance.SetAttackerInfo(this);

        player.ChangeState(PlayerState.Stunned);
    }

    public void ChangeDirection(Player player)
    {
        float diffX = transform.position.x - player.transform.position.x;

        if(diffX < 0 && transform.localScale.x < 0)
        {
            float x = - transform.localScale.x;
            transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
        }
        else if(diffX > 0 && transform.localScale.x > 0)
        {
            float x = -transform.localScale.x;
            transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (curState != YetiState.YDeath && collision.collider.TryGetComponent<Player>(out Player player))
        {
            targetPlayer = player;
            ChangeState(YetiState.YSmash);            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Ground")
        {
            moveDir = -moveDir;
        }
    }
}
