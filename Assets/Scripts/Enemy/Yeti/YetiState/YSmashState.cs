using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YSmashState : StateBase<Yeti>
{
    public YSmashState(Yeti owner) : base(owner)
    {
    }

    public override void Enter()
    {

        owner.ChangeDirection(owner.TargetPlayer);
        owner.ChangeAnimation(YetiState.YSmash);
        owner.Attack(owner.TargetPlayer);
    }

    public override void Update()
    {
        if(owner.CheckCurrentAnimationEnd())
        {
            owner.ChangeState(YetiState.YIdle);
        }
    }
    public override void Exit()
    {

    }

}
