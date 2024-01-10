using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : StateBase<Player>
{
    public IdleState(Player owner) : base(owner)
    {
    }

    float currentEdgeWait;
    float maxEdgeWait = 0.5f;

    public override void Enter()
    {

    }

    public override void Exit()
    {
        currentEdgeWait = 0;
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
        CheckThrow();
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
        if (owner.Rb.velocity.y < -0.1f)
        {
            owner.ChangeState(PlayerState.Fall);
        }
    }

    void CheckOnTheEdge()
    {
        currentEdgeWait += Time.deltaTime;

        if (owner.isGrounded == true && owner.isOnTheEdge == true && currentEdgeWait >= maxEdgeWait)
        {
            owner.ChangeState(PlayerState.OnTheEdge);
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
        if(owner.inven.CurrentHoldItem is Gun == false)
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
