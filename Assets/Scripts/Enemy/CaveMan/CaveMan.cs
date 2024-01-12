using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CaveManState
{
    CSleep,
    CIdle,
    CTrace,
    CAttack,
    CStunned,
    CDeath,
}
public class CaveMan : Enemy
{
    public override void Attack(Player player)
    {
        
    }

    public override void TakeDamage(int Dmg)
    {
        
    }
}
