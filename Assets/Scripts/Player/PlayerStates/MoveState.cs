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
        if (owner.isGrounded && Input.GetButtonDown("Jump"))
        {
            owner.ChangeState(PlayerState.Jump);
        }
    }

    protected void CheckFall()
    {
        if (owner.rb.velocity.y < -0.1f)
        {
            owner.ChangeState(PlayerState.Fall);
        }
    }

    void CheckAttack()
    {
        if (Input.GetButtonDown("Attack"))
        {
            owner.ChangeState(PlayerState.Attack);
        }
    }

    void CheckThrow()
    {
        if (owner.inven.currentHoldItem == null && Input.GetKeyDown(KeyCode.Z))
        {
            owner.throwType = ThrowType.Bomb;
            owner.ChangeState(PlayerState.Throw);
        }

        else if (owner.inven.currentHoldItem != null && Input.GetButtonDown("Attack"))
        {
            owner.throwType = ThrowType.Item;
            owner.ChangeState(PlayerState.Throw);
        }
    }
}
