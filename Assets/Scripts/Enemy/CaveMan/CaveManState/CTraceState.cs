using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTraceState : StateBase<CaveMan>
{
    public CTraceState(CaveMan owner) : base(owner)
    {
    }


    public override void Enter()
    {
        owner.ChangeAnimation(CaveManState.CTrace);
        owner.CheckDirectionToPlayer();
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        owner.Trace();
    }

    void CheckTouchingWall()
    {

    }

    

}
