using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateBase<Player>
{
    public AttackState(Player owner) : base(owner)
    {
    }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        if (owner.CheckCurrentAnimationEnd())
        {
            owner.ChangeState(PlayerState.Idle);
        }
    }
}
