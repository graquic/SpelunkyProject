using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected int hp;
    protected int attDamage;
    
    protected float detectRange;

    



    public abstract void TakeDamage(int Dmg);

    public abstract void Attack(Player player);

    
}
