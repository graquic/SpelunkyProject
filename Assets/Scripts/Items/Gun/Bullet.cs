using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Item
{
    protected int damage = 2;

    [SerializeField] float bulletSpeed;

    [Header("�Ѿ��� ���� �ð�")]
    float currentLifeTime = 0;
    [SerializeField] float returnPoolTime;
    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        rb.AddForce(Vector3.right * bulletSpeed, ForceMode2D.Impulse);

    }

    protected override void Update()
    {
        base.Update();

        currentLifeTime += Time.deltaTime;

        if(currentLifeTime >= returnPoolTime)
        {
            ObjectPoolManager.Instance.ReturnObject(PoolType.Bullet, gameObject);
        }

    }

    protected override void OnDisable()
    {
        currentLifeTime = 0;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Player>(out Player player))
        {
            player.TakeDamage(damage);
            
        }

        else if(collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.TakeDamage(damage);
        }

        ObjectPoolManager.Instance.ReturnObject(PoolType.Bullet, gameObject);
    }
}
