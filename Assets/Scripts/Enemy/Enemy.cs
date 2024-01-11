using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Enemy : MonoBehaviour
{
    [Header("체력")]
    [SerializeField] protected int hp;

    [Header("공격력")]
    [SerializeField] protected int damage;

    [Header("감지 범위")]
    [SerializeField] protected float detectRange;

    protected Rigidbody2D rb;
    protected bool isFlipped;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        ModifyDirection();
    }

    protected void ModifyDirection()
    {
        if (rb.velocity.x > 0 && isFlipped == true)
        {
            isFlipped = false;
            float x = Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector2(x, transform.localScale.y);

        }
        else if (rb.velocity.x < 0 && isFlipped == false)
        {
            isFlipped = true;
            float x = - Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector2(x, transform.localScale.y);

        }
    }


    public abstract void TakeDamage(int Dmg);

    public abstract void Attack(Player player);

    
}
