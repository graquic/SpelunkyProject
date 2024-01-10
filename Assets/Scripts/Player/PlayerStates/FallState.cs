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
        owner.ChangeAnimation(PlayerState.Fall);
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
        if (Mathf.Abs(owner.Rb.velocity.y) < 0.05f && Mathf.Abs(owner.Rb.velocity.x) < 0.05f && owner.isGrounded == true)
        {
            owner.ChangeState(PlayerState.Idle);
        }
    }

    void CheckMove()
    {
        if (Mathf.Abs(owner.Rb.velocity.y) < 0.05f && Mathf.Abs(owner.Rb.velocity.x) >= 0.05f && owner.isGrounded == true)
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
