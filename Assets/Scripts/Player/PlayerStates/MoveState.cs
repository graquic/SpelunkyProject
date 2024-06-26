using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : StateBase<Player>
{
    public MoveState(Player player) : base(player)
    {
    }





    public override void Enter()
    {
        owner.ChangeAnimation(PlayerState.Move);
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        owner.InputMoveAddForce();

        CheckIdle();
        CheckSprint();
        CheckSit();
        CheckJump();
        CheckFall();
        CheckAttack();
        CheckThrow();
        CheckRope();
    }



    protected void CheckIdle()
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) < 0.05f)
        {
            owner.ChangeState(PlayerState.Idle);
        }
    }
    protected void CheckSprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetAxisRaw("Horizontal") != 0)
        {
            owner.ChangeState(PlayerState.Sprint);
        }
    }
    protected void CheckSit()
    {
        if (Input.GetAxisRaw("Vertical") < 0)
        {
            owner.ChangeState(PlayerState.SitDown);
        }
    }

    protected void CheckJump()
    {
        if (owner.IsGrounded == true && Input.GetButtonDown("Jump"))
        {
            owner.ChangeState(PlayerState.Jump);
        }
    }

    protected void CheckFall()
    {
        if (owner.Rb.velocity.y < -0.1f)
        {
            owner.ChangeState(PlayerState.Fall);
        }
    }

    protected void CheckAttack()
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

    protected void CheckThrow()
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

    protected void CheckRope()
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
