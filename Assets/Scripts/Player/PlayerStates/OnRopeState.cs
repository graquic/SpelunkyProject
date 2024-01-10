using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnRopeState : StateBase<Player>
{
    public OnRopeState(Player owner) : base(owner)
    {
    }

    public override void Enter()
    {
        owner.ChangeAnimation(PlayerState.OnRope);
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        if (owner.CheckCurrentAnimationEnd())
        {

        }

    }
}
