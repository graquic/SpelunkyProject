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

        owner.ChangeDirectionToPlayer(owner.TargetPlayer);
        CheckSmashAnimation();
        owner.Attack(owner.TargetPlayer);
    }

    public override void Update()
    {
        if(owner.CheckCurrentAnimationWait())
        {
            owner.ChangeState(YetiState.YIdle);
        }
    }
    public override void Exit()
    {

    }

    void CheckSmashAnimation()
    {
        Vector3 playerPos = owner.TargetPlayer.transform.position;
        float diffX = Mathf.Abs(playerPos.x - owner.transform.position.x);

        if ((playerPos.y > owner.transform.position.y + 1f) && diffX < 0.6f)
        {
            owner.ChangeAnimation("YSmashUpper");
        }
        else
        {
            owner.ChangeAnimation(YetiState.YSmash);
        }
    }

}
