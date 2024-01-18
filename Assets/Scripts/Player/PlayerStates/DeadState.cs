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
        UIManager.Instance.EnableGameOverUI();
    }
    public override void Update()
    {
        if(owner.Rb.velocity.y < -0.1f)
        {
            owner.ChangeAnimation("DeadFall");
        }
        else
        {
            owner.ChangeAnimation(PlayerState.Dead);
        }
    }

    public override void Exit()
    {

    }
}
