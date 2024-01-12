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

        CheckAttack();
        CheckThrow();

    }

    void CheckIdle()
    {
        if (Mathf.Abs(owner.Rb.velocity.y) < 0.05f && Mathf.Abs(owner.Rb.velocity.x) < 0.05f && owner.IsGrounded == true)
        {
            owner.ChangeState(PlayerState.Idle);
        }
    }

    void CheckMove()
    {
        if (Mathf.Abs(owner.Rb.velocity.y) < 0.05f && Mathf.Abs(owner.Rb.velocity.x) >= 0.05f && owner.IsGrounded == true)
        {
            owner.ChangeState(PlayerState.Move);
        }
    }

    void CheckGrabEdge()
    {
        if (owner.IsGrounded == false && owner.isGrabEdge == true)
        {
            owner.ChangeState(PlayerState.GrabEdge);
        }
    }

    void CheckAttack()
    {
        if (owner.inven.CurrentHoldItem == null && Input.GetButtonDown("Attack"))
        {
            owner.ChangeState(PlayerState.Attack);
        }

        else if (owner.inven.CurrentHoldItem is Gun && Input.GetButtonDown("Attack"))
        {
            owner.ChangeState(PlayerState.Attack);
        }
    }

    void CheckThrow()
    {
        if (owner.inven.CurrentHoldItem is Gun == false)
        {
            if (owner.inven.CurrentHoldItem == null && Input.GetKeyDown(KeyCode.Z))
            {
                owner.throwType = ThrowType.Bomb;
                owner.ChangeState(PlayerState.Throw);
            }

            else if (owner.inven.CurrentHoldItem != null && Input.GetButtonDown("Attack"))
            {
                owner.throwType = ThrowType.Item;
                owner.ChangeState(PlayerState.Throw);
            }
        }

    }
}
