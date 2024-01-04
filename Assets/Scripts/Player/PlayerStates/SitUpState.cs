using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitUpState : StateBase<Player>
{
    public SitUpState(Player owner) : base(owner)
    {
    }

    public override void Enter()
    {
        owner.camController.LookUp();
    }

    public override void Exit()
    {
        owner.transform.Find("Sprite").transform.position += new Vector3(0, 0.3f, 0);
    }

    public override void Update()
    {

        if (owner.CheckCurrentAnimationEnd())
        {
            owner.ChangeState(PlayerState.Idle);
        }

    }
}
