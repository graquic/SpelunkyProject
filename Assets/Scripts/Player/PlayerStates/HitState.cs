using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : StateBase<Player>
{
    public HitState(Player owner) : base(owner)
    {
    }

    public override void Enter()
    {
        owner.ChangeAnimation(PlayerState.Hit);
    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }
}
