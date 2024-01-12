using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnedState : StateBase<Player>
{
    public StunnedState(Player owner) : base(owner)
    {
    }
    private enum StunnedType
    {
        Pushed,
        HitFall, 
        Stunned,
    }

    StunnedType curState;

    public override void Enter()
    {
        CheckStunnedPose();
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        switch(curState)
        {
            case StunnedType.Pushed:
                if (owner.IsSmashed == false && owner.Rb.velocity.y < -0.2f)
                {
                    curState = StunnedType.HitFall;
                    owner.ChangeAnimation("HitFall");
                }

                else if (owner.IsSmashed == false && owner.Rb.velocity.x < 0.05f && owner.Rb.velocity.y < 0.05f && owner.IsGrounded == true)
                {
                    curState = StunnedType.Stunned;
                    owner.ChangeAnimation("Stunned");
                }
                break;

            case StunnedType.HitFall:
                if (owner.Rb.velocity.x < 0.05f && owner.Rb.velocity.y < 0.05f && owner.IsGrounded == true)
                {
                    curState = StunnedType.Stunned;
                    owner.ChangeAnimation("Stunned");
                }
            break;

            case StunnedType.Stunned:
                if(owner.CheckCurrentAnimationWait())
                {
                    owner.ChangeState(PlayerState.Idle);
                }
            break;
        }

    }

    void CheckStunnedPose()
    {
        if((owner.Rb.velocity.x < 0 && owner.transform.localScale.x > 0)
            || (owner.Rb.velocity.x > 0 && owner.transform.localScale.x < 0))
        {
            owner.ChangeAnimation("FrontHit");
        }

        else if((owner.Rb.velocity.x > 0 && owner.transform.localScale.x > 0) 
            || (owner.Rb.velocity.x < 0 && owner.transform.localScale.x < 0))
        {
            owner.ChangeAnimation("BackHit");
        }

        curState = StunnedType.Pushed;
    }
}
