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
        owner.SetLayer("Trace");
    }

    public override void Update()
    {
        owner.Trace();
    }

    public override void Exit()
    {
        owner.SetLayer("Enemy");
    }

}
