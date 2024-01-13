using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDeathState : StateBase<CaveMan>
{
    public CDeathState(CaveMan owner) : base(owner)
    {
    }

    private enum CDeathType
    {
        CDeath, CHeld, CThrown
    }

    CDeathType curState;

    public override void Enter()
    {
        owner.ChangeAnimation(CaveManState.CDeath);
        owner.SetIgnorePlayer(true);

        curState = CDeathType.CDeath;
    }
    public override void Update()
    {
        switch(curState)
        {
            case CDeathType.CDeath:
                break;
            case CDeathType.CHeld:
                break;
            case CDeathType.CThrown:
                break;
        }
    }

    public override void Exit()
    {
        
    }

    void CheckOwnerHeld()
    {
        Player player = owner.TargetPlayer;
        
        
    }
    
}
