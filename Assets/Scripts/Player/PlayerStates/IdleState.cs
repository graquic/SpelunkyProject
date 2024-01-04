using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : StateBase<Player>
{
    public IdleState(Player owner) : base(owner)
    {
    }

    float currentWaitTime;
    float maxWaitTime = 0.5f;

    public override void Enter()
    {

    }

    public override void Exit()
    {
        currentWaitTime = 0;
    }

    public override void Update()
    {
        CheckSprint();
        CheckMove();
        CheckJump();
        CheckSit();
        CheckFall();
        CheckOnTheEdge();
        CheckAttack();

    }

    void CheckMove()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            owner.ChangeState(PlayerState.Move);
        }
    }

    void CheckSprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetAxisRaw("Horizontal") != 0)
        {
            owner.ChangeState(PlayerState.Sprint);
        }
    }

    void CheckJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            owner.ChangeState(PlayerState.Jump);
        }
    }

    void CheckSit()
    {
        if (Input.GetAxisRaw("Vertical") < 0)
        {
            owner.ChangeState(PlayerState.SitDown);
        }
    }

    void CheckFall()
    {
        if (owner.rb.velocity.y < -0.1f)
        {
            owner.ChangeState(PlayerState.Fall);
        }
    }

    void CheckOnTheEdge()
    {
        currentWaitTime += Time.deltaTime;

        if (owner.isGrounded == true && owner.isOnTheEdge == true && currentWaitTime >= maxWaitTime)
        {
            owner.ChangeState(PlayerState.OnTheEdge);
        }
    }

    void CheckAttack()
    {
        if(Input.GetButtonDown("Attack"))
        {
            owner.ChangeState(PlayerState.Attack);
        }
    }

}