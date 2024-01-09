using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Cinemachine;

public enum PlayerState
{
    Idle, Move, Sprint, Jump, SitDown, SitUp, Attack, Hit, Stunned, OnTheEdge, GrabEdge, Fall, OnRope, Throw, Dead
}


public enum ThrowType
{
    Bomb,
    Item,
}

public class Player : MonoBehaviour
{
    PlayerState curState;
    public ThrowType throwType;

    StateBase<Player>[] states = new StateBase<Player>[System.Enum.GetValues(typeof(PlayerState)).Length];

    Animator animator;    
    public Transform hand;
    public Transform throwPoint;

    public BoxCollider2D triggeredCol;

    [HideInInspector] public PlayerInventory inven;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public PlayerCameraController camController;
    [SerializeField] TextMeshProUGUI text;  
    [SerializeField] Interactor interactor;


    public GameObject checkGrabEdge;

    private int dir;
    public int Dir { get { return dir; } }

    private int hp;
    public int HP { get { return hp; } }

    [Header("bool ������")]
    public bool isGrounded;
    public bool isOnTheEdge;
    public bool isGrabEdge;
    public bool isFlipped;


    [Header("�̵�(Move)")]
    [SerializeField] float moveSpeed;
    public float MoveSpeed { get { return moveSpeed; } }

    [SerializeField] float maxMoveSpeed;

    float inputX;

    [Header("�޸���(Sprint)")]
    [SerializeField] float sprintSpeed;
    public float SprintSpeed { get { return sprintSpeed; } }
    [SerializeField] float maxSprintSpeed;



    [Header("������(WallJump)")]
    [SerializeField] float jumpPowerX;
    public float jumpXDist { get { return jumpPowerX; } }

    [Header("����(Jump)")]
    [SerializeField] float jumpPowerY;
    public float JumpPowerY { get { return jumpPowerY; } }

    [Header("������(Throw)")]
    [SerializeField] float throwPower;
    public float ThrowPower { get { return throwPower; } }

    


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        camController = GetComponent<PlayerCameraController>();
        inven = GetComponent<PlayerInventory>();
        
        

        states[(int)PlayerState.Idle] = new IdleState(this);
        states[(int)PlayerState.Move] = new MoveState(this);
        states[(int)PlayerState.Sprint] = new SprintState(this);
        states[(int)PlayerState.Jump] = new JumpState(this);
        states[(int)PlayerState.SitDown] = new SitDownState(this);
        states[(int)PlayerState.SitUp] = new SitUpState(this);
        states[(int)PlayerState.Attack] = new AttackState(this);
        states[(int)PlayerState.Hit] = new HitState(this);
        states[(int)PlayerState.Stunned] = new StunnedState(this);
        states[(int)PlayerState.OnTheEdge] = new OnTheEdgeState(this);
        states[(int)PlayerState.GrabEdge] = new GrapEdgeState(this);
        states[(int)PlayerState.Fall] = new FallState(this);
        states[(int)PlayerState.OnRope] = new OnRopeState(this);
        states[(int)PlayerState.Throw] = new ThrowState(this);
        states[(int)PlayerState.Dead] = new DeadState(this);

        curState = PlayerState.Idle;
        states[(int)curState].Enter();
        
        
    }

    void Update()
    {
        CheckDirection();

        states[(int)curState].Update();
        
        //������ �ؽ�Ʈ
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
    }

    void CheckDirection()
    {
        if (inputX < 0 && isFlipped == false)
        {
            isFlipped = true;
            float scaleX = -Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);

            dir = (int)transform.localScale.x;
        }

        else if (inputX > 0 && isFlipped == true)
        {
            isFlipped = false;
            float scaleX = Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);

            dir = (int)transform.localScale.x;
        }
    }

    public void InputMoveAddForce()
    {
        inputX = Input.GetAxis("Horizontal");

        if (rb.velocity.x > maxMoveSpeed && inputX > 0) return;

        if (rb.velocity.x < -maxMoveSpeed && inputX < 0) return;


        rb.AddForce(new Vector2(inputX * MoveSpeed * Time.deltaTime, 0), ForceMode2D.Force);
    }

    public void InputSprintAddForce()
    {
        inputX = Input.GetAxis("Horizontal");

        if (rb.velocity.x > maxSprintSpeed && inputX > 0) return;

        if (rb.velocity.x < -maxSprintSpeed && inputX < 0) return;

        rb.AddForce(new Vector2(inputX * SprintSpeed * Time.deltaTime, 0), ForceMode2D.Force);
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if(damage < 3)
        {
            print(hp);
            ChangeState(PlayerState.Hit);
        }

        else
        {
            print(hp);
            ChangeState(PlayerState.Idle);
        }
        
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



