using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected int hp;
    protected int damage;
    
    protected float detectRange;

    



    public abstract void TakeDamage();

    public abstract void Attack();

    
}
