using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnedState : StateBase<Player>
{
    public StunnedState(Player owner) : base(owner)
    {
    }

    public override void Enter()
    {
        owner.ChangeAnimation(PlayerState.Stunned);
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
