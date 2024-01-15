using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whip : Weapon
{
    [SerializeField] int damage;
    [SerializeField] int pushPower;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            if (collision.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(damage);

                GameObject obj = ObjectPoolManager.Instance.GetObject(PoolType.WhipHitParticle);
                obj.transform.position = GetCollidePos(enemy);
            }

            else if (collision.tag == "Item")
            {
                Vector2 dir = (collision.transform.position - transform.position).normalized;

                collision.GetComponent<Rigidbody2D>().AddForce(dir * pushPower, ForceMode2D.Impulse);

            }
        }
        
    }

    Vector2 GetCollidePos(Enemy enemy)
    {
        Vector2 dir = Vector2.right;

        RaycastHit2D hit;

        hit = Physics2D.Raycast(transform.position, dir, 5);

        if(hit.collider != null)
        {
            return hit.point;
        }

        return transform.position;
    }
}
