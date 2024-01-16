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
        CheckRope();

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
                owner.SetThrowType(ThrowType.Bomb);
                owner.ChangeState(PlayerState.Throw);
            }

            else if (owner.inven.CurrentHoldItem != null && Input.GetButtonDown("Attack"))
            {
                owner.SetThrowType(ThrowType.Item);
                owner.ChangeState(PlayerState.Throw);
            }
        }

    }

    void CheckRope()
    {
        if (owner.inven.GetItemCount(ItemType.Rope) > 0 && Input.GetButtonDown("Rope"))
        {
            GameObject rope = ObjectPoolManager.Instance.GetObject(PoolType.Rope);
            rope.transform.parent = null;
            rope.transform.position = owner.transform.position + new Vector3(0, 0.5f, 0);

            owner.inven.DecreaseItemFromInven(ItemType.Rope, 1);
        }
    }
}
