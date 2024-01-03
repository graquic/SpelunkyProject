using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState
{
    Idle, Move, Jump, Sit, Attack, Hit, Stunned, OnTheEdge, GrabEdge, Fall, Dead
}

public class Player : MonoBehaviour
{
    PlayerState curState;
    StateBase<Player>[] states = new StateBase<Player>[System.Enum.GetValues(typeof(PlayerState)).Length];

    SpriteRenderer sprite;
    Animator animator;
    [SerializeField] TextMeshProUGUI text;


    [HideInInspector] public Rigidbody2D rb;
    public bool isGrounded;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        

        states[(int)PlayerState.Idle] = new IdleState(this);
        states[(int)PlayerState.Move] = new MoveState(this);
        states[(int)PlayerState.Jump] = new JumpState(this);
        states[(int)PlayerState.Sit] = new SitState(this);
        states[(int)PlayerState.Attack] = new AttackState(this);
        states[(int)PlayerState.Hit] = new HitState(this);
        states[(int)PlayerState.Stunned] = new StunnedState(this);
        states[(int)PlayerState.OnTheEdge] = new OnTheEdge(this);
        states[(int)PlayerState.GrabEdge] = new GrapEdgeState(this);
        states[(int)PlayerState.Fall] = new FallState(this);
        states[(int)PlayerState.Dead] = new DeadState(this);

        curState = PlayerState.Idle;
        states[(int)curState].Enter();
        
        
    }

    void Update()
    {
        states[(int)curState].Update();
        text.text = curState.ToString();
    }

    public void ChangeState(PlayerState state)
    {
        states[(int)curState].Exit();
        curState = state;
        animator.SetInteger("State", (int)state);
        states[(int)curState].Enter();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            isGrounded = false;
        }
    }
}


public class IdleState : StateBase<Player>
{
    public IdleState(Player owner) : base(owner)
    {
    }

    public override void Enter()
    {
        owner.isGrounded = true;
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        CheckMove();
        CheckJump();

        
    }

    void CheckMove()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            owner.ChangeState(PlayerState.Move);
        }
    }

    void CheckJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            owner.ChangeState(PlayerState.Jump);
        }
    }

    
}


public class MoveState : StateBase<Player>, IMoveable
{
    public MoveState(Player player) : base(player)
    {
    }

    /*
    float moveSpeed;
    public float MoveSpeed { get { return moveSpeed; } }

    */
    public override void Enter()
    {
        // moveSpeed = 3f;
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        InputMove();

        CheckIdle();
        CheckJump();
        
    }

    public void InputMove()
    {
        float inputX = Input.GetAxis("Horizontal");

        owner.rb.velocity = new Vector2(inputX * IMoveable.MOVESPEED, owner.rb.velocity.y);
    }

    void CheckIdle()
    {
        if (Mathf.Abs(owner.rb.velocity.x) < 0.05f)
        {
            owner.ChangeState(PlayerState.Idle);
        }
    }

    void CheckJump()
    {
        if (owner.isGrounded && Input.GetButtonDown("Jump") )
        {
            owner.ChangeState(PlayerState.Jump);
        }
    }
}

public class JumpState : StateBase<Player>, IMoveable
{
    public JumpState(Player owner) : base(owner)
    {
    }

    float jumpPower;

    public override void Enter()
    {
        jumpPower = 5;

        owner.rb.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        InputMove();

        CheckIdle();
        // CheckMove();
    }

    public void InputMove()
    {
        float inputX = Input.GetAxis("Horizontal");
        
        owner.rb.velocity = new Vector2(inputX * IMoveable.MOVESPEED, owner.rb.velocity.y);
    }

    void CheckIdle()
    {
        if (Mathf.Abs(owner.rb.velocity.x) < 0.05f && Mathf.Abs(owner.rb.velocity.y) < 0.05f) // move -> idle 과는 y값도 확인한다는 점에서 다름
        {
            owner.ChangeState(PlayerState.Idle);
        }
    }

    void CheckMove()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 && owner.isGrounded)
        {
            owner.ChangeState(PlayerState.Move);
        }
    }
}

public class SitState : StateBase<Player>
{
    public SitState(Player owner) : base(owner)
    {
    }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }
}

public class AttackState : StateBase<Player>
{
    public AttackState(Player owner) : base(owner)
    {
    }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }
}

public class HitState : StateBase<Player>
{
    public HitState(Player owner) : base(owner)
    {
    }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }
}

public class StunnedState : StateBase<Player>
{
    public StunnedState(Player owner) : base(owner)
    {
    }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }
}

public class OnTheEdge : StateBase<Player>
{
    public OnTheEdge(Player owner) : base(owner)
    {
    }

    public override void Enter()
    {
        
    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }
}

public class GrapEdgeState : StateBase<Player>
{
    public GrapEdgeState(Player owner) : base(owner)
    {
    }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }
}

public class FallState : StateBase<Player>
{
    public FallState(Player owner) : base(owner)
    {
    }

    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        
    }
}


public class DeadState : StateBase<Player>
{
    public DeadState(Player owner) : base(owner)
    {
    }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }
}
