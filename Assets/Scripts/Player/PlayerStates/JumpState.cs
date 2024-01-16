using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : StateBase<Player>
{
    public JumpState(Player owner) : base(owner)
    {
    }


    public override void Enter()
    {
        owner.ChangeAnimation(PlayerState.Jump);
        owner.Rb.AddForce(new Vector2(0, owner.JumpPowerY), ForceMode2D.Impulse);
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        owner.InputMoveAddForce();

        CheckIdle();
        CheckMove();
        CheckFall();
        CheckGrabEdge();
        CheckAttack();
        CheckThrow();
        CheckRope();
    }


    void CheckIdle()
    {
        if (Mathf.Abs(owner.Rb.velocity.x) == 0 && Mathf.Abs(owner.Rb.velocity.y) == 0) // move -> idle 과는 y값도 확인한다는 점에서 다름
        {
            owner.ChangeState(PlayerState.Idle);
        }
    }

    void CheckMove()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 && owner.Rb.velocity.y == 0 && owner.IsGrounded)
        {
            owner.ChangeState(PlayerState.Move);
        }
    }
    void CheckFall()
    {
        if (owner.Rb.velocity.y < -0.1f)
        {
            owner.ChangeState(PlayerState.Fall);
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

            rope.gameObject.SetActive(true);

            owner.inven.DecreaseItemFromInven(ItemType.Rope, 1);
        }
    }

}
