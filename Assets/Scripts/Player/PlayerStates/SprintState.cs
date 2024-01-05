using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintState : MoveState
{
    public SprintState(Player player) : base(player)
    {
    }

    public override void Update()
    {
        owner.InputSprintAddForce();

        CheckIdle();
        CheckMove();
        CheckSit();
        CheckJump();
        CheckFall();
    }

    void CheckMove()
    {
        if (Input.GetKey(KeyCode.LeftShift) == false && Input.GetAxisRaw("Horizontal") != 0)
        {
            owner.ChangeState(PlayerState.Move);
        }
    }
}
