using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public enum PlayerState
{
    Idle, Move, Sprint, Jump, SitDown, SitUp, Attack, Hit, Stunned, OnTheEdge, GrabEdge, Fall, Dead, 
}

public class Player : MonoBehaviour
{
    PlayerState curState;
    StateBase<Player>[] states = new StateBase<Player>[System.Enum.GetValues(typeof(PlayerState)).Length];

    Animator animator;
    [HideInInspector] public Rigidbody2D rb;
    [SerializeField] TextMeshProUGUI text;
    public GameObject checkGrabEdge;

    [Header("bool 변수들")]
    public bool isGrounded;
    public bool isOnTheEdge;
    public bool isGrabEdge;
    public bool isFlipped;


    [Header("이동(Move)")]
    [SerializeField] float moveSpeed;
    public float MoveSpeed { get { return moveSpeed; } }

    float inputX;

    [Header("달리기(Sprint)")]
    [SerializeField] float sprintSpeed;
    public float SprintSpeed { get { return sprintSpeed; } }

    

    [Header("점프(Jump)")]
    [SerializeField] float jumpPowerX;
    public float JumpPowerX { get { return jumpPowerX; } }

    [SerializeField] float jumpPowerY;
    public float JumpPowerY { get { return jumpPowerY; } }
    


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        

        states[(int)PlayerState.Idle] = new IdleState(this);
        states[(int)PlayerState.Move] = new MoveState(this);
        states[(int)PlayerState.Sprint] = new SprintState(this);
        states[(int)PlayerState.Jump] = new JumpState(this);
        states[(int)PlayerState.SitDown] = new SitDownState(this);
        states[(int)PlayerState.SitUp] = new SitUpState(this);
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
        checkGrabEdge.transform.position = new Vector2(0.5f, 0.17f);

        CheckDirection();

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

    public bool CheckCurrentAnimationEnd()
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
            //animator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }

    void CheckDirection()
    {
        if(inputX < 0 && isFlipped == false)
        {
            isFlipped = true;
            float scaleX = -Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector2(scaleX, transform.localScale.y);
        }

        else if(inputX > 0 && isFlipped == true)
        {
            isFlipped = false;
            float scaleX = Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector2(scaleX, transform.localScale.y);
        }
    }

    public void InputMove()
    {
        inputX = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(inputX * MoveSpeed, rb.velocity.y);
    }

    public void InputSprint()
    {
        inputX = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(inputX * SprintSpeed, rb.velocity.y);
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
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

    float currentWaitTime;
    float maxWaitTime = 0.5f;

    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        currentWaitTime = 0;
    }

    public override void Update()
    {
        CheckSprint();
        CheckMove();
        CheckJump();
        CheckSit();
        CheckFall();
        CheckOnTheEdge();
        
        
    }

    void CheckMove()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 )
        {
            owner.ChangeState(PlayerState.Move);
        }
    }

    void CheckSprint()
    {
        if(Input.GetKey(KeyCode.LeftShift) && Input.GetAxisRaw("Horizontal") != 0)
        {
            owner.ChangeState(PlayerState.Sprint);
        }
    }

    void CheckJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            owner.ChangeState(PlayerState.Jump);
        }
    }

    void CheckSit()
    {
        if(Input.GetAxisRaw("Vertical") < 0)
        {
            owner.ChangeState(PlayerState.SitDown);
        }
    }

    void CheckFall()
    {
        if(owner.rb.velocity.y < -0.1f)
        {
            owner.ChangeState(PlayerState.Fall);
        }
    }

    void CheckOnTheEdge()
    {
        currentWaitTime += Time.deltaTime;

        if(owner.isGrounded == true && owner.isOnTheEdge ==  true && currentWaitTime >= maxWaitTime)
        {
            owner.ChangeState(PlayerState.OnTheEdge);
        }
    }
    
}


public class MoveState : StateBase<Player>
{
    public MoveState(Player player) : base(player)
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
        owner.InputMove();

        CheckIdle();
        CheckSprint();
        CheckSit();
        CheckJump();
        CheckFall();
        
    }

    

    protected void CheckIdle()
    {
        if (Mathf.Abs(owner.rb.velocity.x) < 0.05f)
        {
            owner.ChangeState(PlayerState.Idle);
        }
    }
    protected void CheckSprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetAxisRaw("Horizontal") != 0)
        {
            owner.ChangeState(PlayerState.Sprint);
        }
    }
    protected void CheckSit()
    {
        if (Input.GetAxisRaw("Vertical") < 0)
        {
            owner.ChangeState(PlayerState.SitDown);
        }
    }

    protected void CheckJump()
    {
        if (owner.isGrounded && Input.GetButtonDown("Jump") )
        {
            owner.ChangeState(PlayerState.Jump);
        }
    }

    protected void CheckFall()
    {
        if (owner.rb.velocity.y < -0.1f)
        {
            owner.ChangeState(PlayerState.Fall);
        }
    }
}

public class SprintState : MoveState
{
    public SprintState(Player player) : base(player)
    {
    }

    public override void Update()
    {
        owner.InputSprint();

        CheckIdle();
        CheckMove();
        CheckSit();
        CheckJump();
        CheckFall();
    }

    void CheckMove()
    {
        if(Input.GetKey(KeyCode.LeftShift) == false && Input.GetAxisRaw("Horizontal") != 0)
        {
            owner.ChangeState(PlayerState.Move);
        }
    }
}
public class JumpState : StateBase<Player>
{
    public JumpState(Player owner) : base(owner)
    {
    }


    public override void Enter()
    {
        owner.rb.AddForce(new Vector2(owner.rb.velocity.x, owner.JumpPowerY), ForceMode2D.Impulse);
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        owner.InputMove();

        CheckIdle();
        CheckMove();
        CheckFall();
        CheckGrabEdge();
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
        if (Input.GetAxisRaw("Horizontal") != 0 && owner.rb.velocity.y == 0 && owner.isGrounded)
        {
            owner.ChangeState(PlayerState.Move);
        }
    }
    void CheckFall()
    {
        if (owner.rb.velocity.y < -0.1f)
        {
            owner.ChangeState(PlayerState.Fall);
        }
    }

    void CheckGrabEdge()
    {
        if(owner.isGrounded == false && owner.isGrabEdge == true)
        {
            owner.ChangeState(PlayerState.GrabEdge);
        }
    }

}

public class SitDownState : StateBase<Player>
{
    public SitDownState(Player owner) : base(owner)
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
        CheckSitUp();
        CheckFall();
    }

    void CheckSitUp()
    {
        if(Input.GetAxisRaw("Vertical") >= 0)
        {
            owner.ChangeState(PlayerState.SitUp);
        }
    }

    void CheckFall()
    {
        if (owner.rb.velocity.y < -0.1f)
        {
            owner.ChangeState(PlayerState.Fall);
        }
    }

}

public class SitUpState : StateBase<Player>
{
    public SitUpState(Player owner) : base(owner)
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
        
        if (owner.CheckCurrentAnimationEnd())
        {
            owner.ChangeState(PlayerState.Idle);
        }
        
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

public class OnTheEdge : IdleState
{
    public OnTheEdge(Player owner) : base(owner)
    {
    }

    
}

public class GrapEdgeState : StateBase<Player>
{
    public GrapEdgeState(Player owner) : base(owner)
    {
    }

    float jumpYdist = 3f;

    public override void Enter()
    {

    }

    public override void Exit()
    {
        owner.isGrabEdge = false;
        // owner.boxcol.isTrigger = false;
    }

    public override void Update()
    {
        CheckFall();
        CheckJump();
    }

    void CheckJump()
    {
        if(owner.isFlipped == true)
        {
            if(Input.GetAxis("Horizontal") > 0 && Input.GetButtonDown("Jump"))
            {
                Vector3 dir = new Vector3(owner.transform.position.x + owner.JumpPowerX, owner.transform.position.y + jumpYdist) - owner.transform.position;
                dir = dir.normalized;
                owner.rb.AddForce(dir * owner.JumpPowerX, ForceMode2D.Impulse);
                owner.ChangeState(PlayerState.Jump);
            }
        }
        
        else
        {
            if (Input.GetAxis("Horizontal") < 0 && Input.GetButtonDown("Jump"))
            {
                Vector3 dir = new Vector3(owner.transform.position.x - owner.JumpPowerX, owner.transform.position.y + jumpYdist) - owner.transform.position;
                dir = dir.normalized;
                owner.rb.AddForce(dir * owner.JumpPowerX, ForceMode2D.Impulse);
                owner.ChangeState(PlayerState.Jump);
            }
        }
    }

    void CheckFall()
    {
        if (owner.rb.velocity.y < 0)
        {
            owner.ChangeState(PlayerState.Fall);
        }

        if (Input.GetAxisRaw("Vertical") < 0 && Input.GetButtonDown("Jump"))
        {
            owner.checkGrabEdge.GetComponent<BoxCollider2D>().isTrigger = true; // TODO
            owner.ChangeState(PlayerState.Fall);
        }
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
        owner.InputMove();

        CheckIdle();
        CheckGrabEdge();
    }

    void CheckIdle()
    {
        if(owner.rb.velocity.y == 0 && owner.isGrounded == true)
        {
            owner.ChangeState(PlayerState.Idle);
        }
    }

    void CheckGrabEdge()
    {
        if (owner.isGrounded == false && owner.isGrabEdge == true)
        {
            owner.ChangeState(PlayerState.GrabEdge);
        }
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
