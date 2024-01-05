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
        owner.rb.AddForce(new Vector2(0, owner.JumpPowerY), ForceMode2D.Impulse);
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
    }


    void CheckIdle()
    {
        if (Mathf.Abs(owner.rb.velocity.x) < 0.05f && Mathf.Abs(owner.rb.velocity.y) < 0.05f) // move -> idle 과는 y값도 확인한다는 점에서 다름
        {
            owner.ChangeState(PlayerState.Idle);
        }
    }

    void CheckMove()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 && owner.rb.velocity.y == 0 && owner.isGrounded)
        {
            owner.ChangeState(PlayerState.Move);
        }
    }
    void CheckFall()
    {
        if (owner.rb.velocity.y < -0.1f)
        {
            owner.ChangeState(PlayerState.Fall);
        }
    }

    void CheckGrabEdge()
    {
        if (owner.isGrounded == false && owner.isGrabEdge == true)
        {
            owner.ChangeState(PlayerState.GrabEdge);
        }
    }

    void CheckAttack()
    {
        if (Input.GetButtonDown("Attack"))
        {
            owner.ChangeState(PlayerState.Attack);
        }
    }

}
