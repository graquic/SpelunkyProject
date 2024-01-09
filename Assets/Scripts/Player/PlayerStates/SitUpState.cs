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

        // SetBoxCol();
    }

    public override void Update()
    {

        if (owner.CheckCurrentAnimationEnd())
        {
            owner.ChangeState(PlayerState.Idle);
        }

    }
    
    void SetBoxCol()
    {
        owner.triggeredCol.offset = new Vector2(-0.04287338f, -0.3467688f);
        owner.triggeredCol.size = new Vector2(0.7462112f, 0.1726592f);
    }
    
}
