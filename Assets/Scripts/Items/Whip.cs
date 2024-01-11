using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whip : Weapon
{
    [SerializeField] int damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.TakeDamage(damage);
        }
    }
}
