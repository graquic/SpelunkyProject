using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum CaveManState
{
    CSleep, CIdle, CMove, CTrace, CStunned, CDeath,

}
public class CaveMan : Enemy, IHoldable
{
    CaveManState curState;
    public CaveManState CurState { get { return curState; } }

    Animator animator;
    Player targetPlayer;
    public Player TargetPlayer { get { return targetPlayer; } }

    [SerializeField] float moveSpeed;
    [SerializeField] float traceSpeed;
    public float TraceSpeed { get { return traceSpeed; } }

    
    [SerializeField] TextMeshProUGUI testText;
    [SerializeField] CapsuleCollider2D bodyCol;
    public CapsuleCollider2D BodyCol { get { return bodyCol; } }
    [SerializeField] GameObject headHitPoint;
    [SerializeField] BoxCollider2D frontDetectCol;
    public BoxCollider2D FrontDetectCol { get { return frontDetectCol; } }

    StateBase<CaveMan>[] states = new StateBase<CaveMan>[Enum.GetValues(typeof(CaveManState)).Length];

    int moveDir;
    public int MoveDir { get { return moveDir; } }

    Vector2 traceDir;

    bool isCollideWall;
    public bool IsCollideWall { get { return isCollideWall; } }

    [Header("충돌 시의 반동")]
    [SerializeField] float pushPower;
    public float PushPower { get { return pushPower; } }

    [SerializeField] Vector2 pushedDir;
    public Vector2 PushedDir { get { return pushedDir; } }



    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();

        states[(int)CaveManState.CSleep] = new CSleepState(this);
        states[(int)CaveManState.CIdle] = new CIdleState(this);
        states[(int)CaveManState.CMove] = new CMoveState(this);
        states[(int)CaveManState.CTrace] = new CTraceState(this);
        states[(int)CaveManState.CStunned] = new CStunnedState(this);
        states[(int)CaveManState.CDeath] = new CDeathState(this);

        curState = CaveManState.CSleep;
        states[(int)curState].Enter();
    }

    private void Start()
    {
        targetPlayer = GameManager.Instance.player;
    }

    protected override void Update()
    {

        base.Update();

        states[(int)curState].Update();
    }

    public void ChangeState(CaveManState state)
    {
        states[(int)curState].Exit();
        curState = state;
        states[(int)curState].Enter();
    }

    public void ChangeState(CaveManState state, float waitTime)
    {
        StartCoroutine(ChangeStateCoroutine(state, waitTime));
    }

    IEnumerator ChangeStateCoroutine(CaveManState state, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        states[(int)curState].Exit();
        curState = state;
        states[(int)curState].Enter();
    }

    public void ChangeAnimation(CaveManState state)
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

    public bool CheckCurrentAnimationWait()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Wait");
    }

    public void SetRandomMoveDirection()
    {
        moveDir = UnityEngine.Random.Range(0, 2) * 2 - 1;
    }

    public void SetMoveDirection(int dir)
    {
        moveDir = dir;
    }

    public void SetIsCollideWall(bool isCollideWall)
    {
        this.isCollideWall = isCollideWall;
    }

    public void Move()
    {
        rb.velocity = new Vector2(moveDir * moveSpeed, rb.velocity.y);
        ModifyDirection();
    }

    public void Trace()
    {
        rb.velocity = traceDir * TraceSpeed + new Vector2(0, rb.velocity.y);
        ModifyDirection();
    }
    public void CheckDirectionToPlayer()
    {
        float diffX = transform.position.x - TargetPlayer.transform.position.x;

        if (diffX < 0)
        {
            traceDir = Vector2.right;
        }

        else if (diffX > 0)
        {
            traceDir = Vector2.left;
        }

        else { print("diffX == 0"); traceDir = Vector2.right; }
        
    }

    public void SetIgnorePlayer(bool setIgnored)
    {
        if(setIgnored == true)
        {
            SetHitPoint(false);
            SetLayer("IgnorePlayer");
        }

        else
        {
            SetHitPoint(true);
            SetLayer("Enemy");
        }
        
    }
    public override void Attack(Player player)
    {
        
    }

    public override void TakeDamage(int Dmg)
    {
        if (curState != CaveManState.CDeath)
        {
            hp -= Dmg;

            if (hp <= 0)
            {
                GameManager.Instance.AddCurScore(score);
                ChangeState(CaveManState.CDeath);
                return;
            }

            if (Dmg <= 1)
            { 
                PushBack();
                if(curState != CaveManState.CStunned)
                {
                    ChangeState(CaveManState.CTrace);
                }
                
            }
            else 
            {
                if (curState != CaveManState.CStunned)
                {
                    ChangeState(CaveManState.CStunned);
                    ChangeAnimation(CaveManState.CStunned);
                }
                    
            }

            
        }
    }
    public void PushBack(Vector2 dir, float pushPower)
    {
        rb.AddForce(dir.normalized * pushPower, ForceMode2D.Impulse);
    }

    void PushBack()
    {
        float dir = UnityEngine.Random.Range(0, 2) * 2 - 1;
        int pushBackPower = UnityEngine.Random.Range(2, 5);
        if (dir < 0)
        {
            rb.AddForce(new Vector2(-pushBackPower, 3f), ForceMode2D.Impulse);
        }
        else if (dir > 0)
        {
            rb.AddForce(new Vector2(pushBackPower, 3f), ForceMode2D.Impulse);
        }

        else { print("error"); rb.AddForce(new Vector2(pushBackPower, 1f), ForceMode2D.Impulse); }
    }

    public void SetHitPoint(bool isOn)
    {
        headHitPoint.SetActive(isOn);
    }

    public void SetLayer(string layerName)
    {
        gameObject.layer = LayerMask.NameToLayer(layerName);
    }

    public void SetHoldItem(Player player)
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(curState != CaveManState.CDeath && curState != CaveManState.CStunned)
        {
            if (collision.collider.tag == "Player")
            {
                Player player = collision.collider.GetComponent<Player>();
                player.TakeDamage(damage, transform.position);

                if (curState == CaveManState.CTrace)
                {
                    PushBack(-PushedDir, pushPower);
                    if (player.CurState == PlayerState.Dead) { return; }
                    ChangeState(CaveManState.CStunned);
                }
            }
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }
    }

}
