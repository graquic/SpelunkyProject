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
    Idle, Move, Sprint, Jump, SitDown, SitUp, Attack, Hit, Stunned, OnTheEdge, GrabEdge, Fall, Dead
}

public class Player : MonoBehaviour
{
    PlayerState curState;

    StateBase<Player>[] states = new StateBase<Player>[System.Enum.GetValues(typeof(PlayerState)).Length];

    Animator animator;
    PlayerInventory inven;

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public PlayerCameraController camController;
    [SerializeField] TextMeshProUGUI text;
    
    
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

    public float inputX;
    [SerializeField] float maxMoveSpeed;

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
        states[(int)PlayerState.Dead] = new DeadState(this);

        curState = PlayerState.Idle;
        states[(int)curState].Enter();
        
        
    }

    void Update()
    {
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
    }

    void CheckDirection()
    {
        if (inputX < 0 && isFlipped == false)
        {
            isFlipped = true;
            float scaleX = -Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector2(scaleX, transform.localScale.y);

            dir = (int)transform.localScale.x;
        }

        else if (inputX > 0 && isFlipped == true)
        {
            isFlipped = false;
            float scaleX = Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector2(scaleX, transform.localScale.y);

            dir = (int)transform.localScale.x;
        }
    }

    public void InputMoveAddForce( float time)
    {
        inputX = Input.GetAxis("Horizontal");

        if (rb.velocity.x > maxMoveSpeed && inputX > 0) return;

        if (rb.velocity.x < -maxMoveSpeed && inputX < 0) return;


        rb.AddForce(new Vector2(inputX * MoveSpeed * time, 0), ForceMode2D.Force);
    }

    public void InputSprintAddForce(float time)
    {
        inputX = Input.GetAxis("Horizontal");

        if (rb.velocity.x > maxSprintSpeed && inputX > 0) return;

        if (rb.velocity.x < -maxSprintSpeed && inputX < 0) return;

        rb.AddForce(new Vector2(inputX * SprintSpeed * time, 0), ForceMode2D.Force);
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if(damage < 3)
        {
            ChangeState(PlayerState.Hit);
        }

        else
        {
            ChangeState(PlayerState.Stunned);
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


