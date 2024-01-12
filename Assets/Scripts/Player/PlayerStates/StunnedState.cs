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
        CheckStunnedPose();
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
    }
}
