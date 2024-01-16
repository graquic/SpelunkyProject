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

public class Player : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI stateText;

    PlayerState curState;
    ThrowType throwType;
    public ThrowType ThrowType { get { return throwType; } }
    [HideInInspector] public PlayerInventory inven;

    StateBase<Player>[] states = new StateBase<Player>[System.Enum.GetValues(typeof(PlayerState)).Length];

    Animator animator;    
    public Transform hand;
    public Transform throwPoint;

    [SerializeField] List<PhysicsMaterial2D> pMaterials;
    public List<PhysicsMaterial2D> PMaterials { get { return pMaterials; } }

    Rigidbody2D rb;
    public Rigidbody2D Rb { get { return rb; } }
    

    [HideInInspector] public PlayerCameraController camController;

    private int dir;
    public int Dir { get { return dir; } }

    

    [Header("bool 변수들")]
    [SerializeField] private bool isGrounded;
    public bool IsGrounded { get { return isGrounded; } }
    [SerializeField] private bool isOnTheEdge;
    public bool IsOnTheEdge { get { return isOnTheEdge; } }
    public bool isGrabEdge;
    public bool isFlipped;
    public bool isOnRope;

    [Header("무적 시간")]
    [SerializeField] float invincibleTime;
    bool isInvincible;
    public bool IsInvincible { get {  return isInvincible; } }
    
    [Header("이동(Move)")]
    [SerializeField] float moveSpeed;
    public float MoveSpeed { get { return moveSpeed; } }

    [SerializeField] float maxMoveSpeed;

    float inputX;

    [Header("달리기(Sprint)")]
    [SerializeField] float sprintSpeed;
    public float SprintSpeed { get { return sprintSpeed; } }
    [SerializeField] float maxSprintSpeed;



    [Header("벽점프(WallJump)")]
    [SerializeField] float jumpPowerX;
    public float jumpXDist { get { return jumpPowerX; } }

    [Header("점프(Jump)")]
    [SerializeField] float jumpPowerY;
    public float JumpPowerY { get { return jumpPowerY; } }

    [Header("던지기(Throw)")]
    [SerializeField] float throwPower;
    public float ThrowPower { get { return throwPower; } }

    [Header("밀려나는 정도")]
    [SerializeField] float pushBackPower;

    [Header("밟기 데미지")]
    [SerializeField] int stepDamage;
    public int StepDamage { get { return stepDamage; } }


    private bool isSmashed;
    public bool IsSmashed { get { return isSmashed; } }

    public GameObject touchedRope;


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

        dir = (int)transform.localScale.x;
    }

    void Update()
    {
        stateText.text = curState.ToString();

        CheckDirection();
        SetPhysicsMaterial();

        states[(int)curState].Update();
        
        //디버깅용 텍스트
    }

    public void ChangeState(PlayerState state)
    {
        states[(int)curState].Exit();
        curState = state;
        states[(int)curState].Enter();
    }

    public void ChangeAnimation(PlayerState state)
    {
        animator.Play(state.ToString());
    }

    public void ChangeAnimation(AttackType state)
    {
        animator.Play(state.ToString());
    }

    public void ChangeAnimation(String animationName)
    {
        animator.Play(animationName);
    }

    

    public bool CheckCurrentAnimationEnd()
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
    }
    public bool CheckCurrentAnimationWait()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Wait");
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

        if (Rb.velocity.x > maxMoveSpeed && inputX > 0) return;

        if (Rb.velocity.x < -maxMoveSpeed && inputX < 0) return;


        Rb.AddForce(new Vector2(inputX * MoveSpeed * Time.deltaTime, 0), ForceMode2D.Force);
    }

    public void InputSprintAddForce()
    {
        inputX = Input.GetAxis("Horizontal");

        if (Rb.velocity.x > maxSprintSpeed && inputX > 0) return;

        if (Rb.velocity.x < -maxSprintSpeed && inputX < 0) return;

        Rb.AddForce(new Vector2(inputX * SprintSpeed * Time.deltaTime, 0), ForceMode2D.Force);
    }

    public void SetHoldItemSetting(Item curItem, bool isAwayFromHand)
    {
        if(isAwayFromHand == false)
        {
            curItem.GetComponent<Collider2D>().isTrigger = true;
            curItem.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            curItem.GetComponent<Rigidbody2D>().gravityScale = 0;
        }

        else
        {
            curItem.GetComponent<Collider2D>().isTrigger = false;
            curItem.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            curItem.GetComponent<Rigidbody2D>().gravityScale = 1;
        }
    }
    public void TakeDamage(int damage)
    {
        if (curState == PlayerState.Dead) { return; }

        if (isInvincible == true) { return; }

        GameManager.Instance.SetPlayerDamaged(damage);

        if(GameManager.Instance.PlayerHp <= 0)
        {
            ChangeState(PlayerState.Dead);
            return;
        }

        animator.SetTrigger("Hit");

        if(damage > 2)
        {
            ChangeState(PlayerState.Stunned);
        }

        GameManager.Instance.hpChanged.Invoke();
        StartCoroutine(SetInvincible());
    }

    public void TakeDamage(int damage, Vector3 AttackerPos)
    {
        if (curState == PlayerState.Dead) { return; }

        if (isInvincible == true) { return; }

        GameManager.Instance.SetPlayerDamaged(damage);

        if (GameManager.Instance.PlayerHp <= 0)
        {
            ChangeState(PlayerState.Dead);
            return;
        }

        animator.SetTrigger("Hit");

        GameManager.Instance.hpChanged.Invoke();
        StartCoroutine(SetInvincible());
        PushBack(AttackerPos);

        if(damage > 2)
        {
            DropCurrentItem();
            ChangeState(PlayerState.Stunned);
        }

        StartCoroutine(SetInvincible());
    }

    IEnumerator SetInvincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }

    void DropCurrentItem()
    {
        if(inven.CurrentHoldItem != null)
        {
            inven.SetCurrentHoldItem(null);
        }
    }

    void PushBack(Vector3 AttackerPos)
    {
        float diffX = transform.position.x - AttackerPos.x;
        if (diffX < 0)
        {
            rb.AddForce(new Vector2(-pushBackPower, 8f), ForceMode2D.Impulse);
        }
        else if (diffX > 0)
        {
            rb.AddForce(new Vector2(pushBackPower, 8f), ForceMode2D.Impulse);
        }

        else { print("error"); rb.AddForce(new Vector2(pushBackPower, 1f), ForceMode2D.Impulse); }
    }

    public void SetThrowType(ThrowType type)
    {
        throwType = type;
    }

    public void SetPhysicsMaterial()
    {
        if(curState == PlayerState.Jump || curState == PlayerState.Fall)
        {
            rb.sharedMaterial = pMaterials.Find((x) => x.name == "NoFriction");
        }

        else if(curState == PlayerState.Stunned)
        {
            rb.sharedMaterial = pMaterials.Find((x) => x.name == "StunnedBounce");
        }

        else
        {
            rb.sharedMaterial = pMaterials.Find((x) => x.name == "PlayerFriction");
        }
    }

    public void SetIsSmashed(bool isSmashed)
    {
        this.isSmashed = isSmashed;
    }
    public void SetIsOnTheEdge(bool isOnTheEdge)
    {
        this.isOnTheEdge = isOnTheEdge;
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            isGrounded = true;
        }
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(isOnRope == false)
        {
            if (collision.tag == "Rope" && Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0f)
            {
                touchedRope = collision.gameObject;
                ChangeState(PlayerState.OnRope);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            isGrounded = false;
        }
        
        if(collision.tag == "Rope")
        {
            if(isGrounded == false)
            {
                ChangeState(PlayerState.Fall);
            }
            else
            {
                ChangeState(PlayerState.Idle);
            }
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    { 
        if (curState == PlayerState.Dead) { return; }

        if(isSmashed == true)
        {
            if (collision.collider.tag == "Ground" || collision.collider.tag == "Wall")
            {
                isSmashed = false;
                isInvincible = false;
                TakeDamage(GameManager.Instance.AttackerInfo.Damage);
                GameManager.Instance.SetAttackerInfo(null);
            }
        }

        if(collision.collider.tag == "Enemy")
        {
            if(collision.collider.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            {
                if (rb.velocity.y > 10f)
                {
                    TakeDamage(3);
                }
            }
        }
    }
}



