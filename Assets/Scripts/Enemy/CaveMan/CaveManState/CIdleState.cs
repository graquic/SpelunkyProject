using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class CIdleState : StateBase<CaveMan>
{
    public CIdleState(CaveMan owner) : base(owner)
    {
    }

    float maxWaitToMoveTime;
    float currentWaitToMoveTime;

    public override void Enter()
    {
        owner.ChangeAnimation(CaveManState.CIdle);

        maxWaitToMoveTime = 0;
        currentWaitToMoveTime = 0;
    }
    public override void Update()
    {
        currentWaitToMoveTime += Time.deltaTime;

        SetWaitTime();

        CheckMove();
        CheckTrace();
    }

    public override void Exit()
    {
        maxWaitToMoveTime = 0;
        currentWaitToMoveTime = 0;
    }

    void SetWaitTime()
    {
        if (maxWaitToMoveTime == 0)
        {
            maxWaitToMoveTime = Random.Range(1, 3);
        }
    }

    void CheckMove()
    {
        if (currentWaitToMoveTime >= maxWaitToMoveTime)
        {
            owner.ChangeState(CaveManState.CMove);
        }
    }

    void CheckTrace()
    {
        float dist = Vector2.Distance(owner.transform.position, owner.TargetPlayer.transform.position);

        if(CheckFrontPlayer() == true && dist < owner.DetectRange)
        {
            owner.ChangeState(CaveManState.CTrace);
        }
    }
    bool CheckFrontPlayer()
    {
        float diffX = owner.transform.position.x - owner.TargetPlayer.transform.position.x;

        if (diffX > 0 && owner.transform.localScale.x < 0)
        {
            return true;
        }
        else if (diffX < 0 && owner.transform.localScale.x > 0)
        {
            return true;
        }

        return false;
    }

}
