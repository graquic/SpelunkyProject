using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : StateBase<Player>
{
    public DeadState(Player owner) : base(owner)
    {
    }

    public override void Enter()
    {
        owner.ChangeAnimation(PlayerState.Dead);
    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }
}
