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
        owner.SetIgnorePlayer(true);

        owner.ChangeAnimation(CaveManState.CDeath);
        curState = CDeathType.CDeath;

        GameManager.Instance.AddCurScore(owner.Score);
    }
    public override void Update()
    {
        switch(curState)
        {
            case CDeathType.CDeath:
                owner.ChangeAnimation(CaveManState.CDeath);
                CheckDeathPose();
                break;
            case CDeathType.CHeld:
                break;
            case CDeathType.CThrown:
                if(Mathf.Abs(owner.Rb.velocity.x) < 0.1f && Mathf.Abs(owner.Rb.velocity.y) < 0.1f 
                    && owner.BodyCol.IsTouchingLayers(LayerMask.GetMask("Ground")))
                {
                    curState = CDeathType.CDeath;
                    owner.ChangeAnimation(CaveManState.CDeath);
                }
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

    void CheckDeathPose()
    {
        if(owner.BodyCol.IsTouchingLayers(LayerMask.GetMask("Ground")) == false)
        {
            if (owner.transform.localScale.x > 0)
            {
                if (owner.Rb.velocity.x > 0.1f)
                {
                    owner.ChangeAnimation("CBackHit");
                }

                else if (owner.Rb.velocity.x < 0.1f)
                {
                    owner.ChangeAnimation("CFrontHit");
                }

                curState = CDeathType.CThrown;
            }

            else if (owner.transform.localScale.x < 0)
            {
                if (owner.Rb.velocity.x < 0.1f)
                {
                    owner.ChangeAnimation("CBackHit");
                }

                else if (owner.Rb.velocity.x > 0.1f)
                {
                    owner.ChangeAnimation("CFrontHit");
                }

                curState = CDeathType.CThrown;
            }
        }

        
    }
    
}
