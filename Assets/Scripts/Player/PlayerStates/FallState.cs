using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : StateBase<Player>
{
    public FallState(Player owner) : base(owner)
    {
    }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        owner.InputMoveAddForce();

        CheckIdle();
        CheckMove();
        CheckGrabEdge();
    }

    void CheckIdle()
    {
        if (owner.rb.velocity.y == 0 && owner.rb.velocity.x < 0.05f && owner.isGrounded == true)
        {
            owner.ChangeState(PlayerState.Idle);
        }
    }

    void CheckMove()
    {
        if (owner.rb.velocity.y == 0 && owner.rb.velocity.x >= 0.05f && owner.isGrounded == true)
        {
            owner.ChangeState(PlayerState.Move);
        }
    }

    void CheckGrabEdge()
    {
        if (owner.isGrounded == false && owner.isGrabEdge == true)
        {
            owner.ChangeState(PlayerState.GrabEdge);
        }
    }
}
