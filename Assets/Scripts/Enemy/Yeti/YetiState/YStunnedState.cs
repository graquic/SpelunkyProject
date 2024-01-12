using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YStunnedState : StateBase<Yeti>
{
    public YStunnedState(Yeti owner) : base(owner)
    {
    }

    public override void Enter()
    {
        owner.ChangeAnimation(YetiState.YStunned);
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        if(owner.CheckCurrentAnimationWait())
        {
            owner.ChangeState(YetiState.YIdle);
        }
    }
}
