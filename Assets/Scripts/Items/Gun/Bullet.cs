using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Item
{
    protected int damage = 2;

    [SerializeField] float bulletSpeed;

    [Header("총알의 생존 시간")]
    float currentLifeTime = 0;
    [SerializeField] float returnPoolTime;
    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        int dir = GameManager.Instance.player.Dir;
        rb.AddForce(Vector3.right * bulletSpeed * dir, ForceMode2D.Impulse);

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
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<Player>(out Player player))
        {
            player.TakeDamage(damage);

        }

        else if (collision.collider.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.TakeDamage(damage);
        }

        print(collision.gameObject.name);
        GameObject hitParticle = ObjectPoolManager.Instance.GetObject(PoolType.HitParticle);
        hitParticle.transform.position = transform.position;

        ObjectPoolManager.Instance.ReturnObject(PoolType.Bullet, gameObject);
    }
}
