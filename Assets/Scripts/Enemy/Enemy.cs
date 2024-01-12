using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Enemy : MonoBehaviour
{
    [Header("체력")]
    [SerializeField] protected int hp;
    public int Hp { get { return hp; } }

    [Header("공격력")]
    [SerializeField] protected int damage;
    public int Damage { get { return damage; } }

    [Header("감지 범위")]
    [SerializeField] protected float detectRange;

    protected Rigidbody2D rb;
    protected int dir;
    public int Dir { get { return dir; } }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        // ModifyDirection();
    }

    protected void ModifyDirection()
    {
        if (rb.velocity.x > 0.1f && transform.localScale.x < 0)
        {
            float x = Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
        }
        else if (rb.velocity.x < 0.1f && transform.localScale.x > 0)
        {
            float x = - Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
        }
        
        dir = (int)transform.localScale.x;
    }


    public abstract void TakeDamage(int Dmg);

    public abstract void Attack(Player player);

    
}
