using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintState : MoveState
{
    public SprintState(Player player) : base(player)
    {
    }
    public override void Enter()
    {
        owner.ChangeAnimation(PlayerState.Sprint);
    }

    public override void Update()
    {
        owner.InputSprintAddForce();

        base.Update();

        CheckMove();
    }

    void CheckMove()
    {
        if (Input.GetKey(KeyCode.LeftShift) == false && Input.GetAxisRaw("Horizontal") != 0)
        {
            owner.ChangeState(PlayerState.Move);
        }
    }

    
}
