using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMoveState : StateBase<CaveMan>
{
    public CMoveState(CaveMan owner) : base(owner)
    {
    }

    float maxMoveTime;
    float currentMoveTime;
    public override void Enter()
    {
        owner.ChangeAnimation(CaveManState.CMove);

        maxMoveTime = 0;
        currentMoveTime = 0;

        owner.SetRandomMoveDirection();
    }
    public override void Update()
    {
        SetMoveTime();
        currentMoveTime += Time.deltaTime;
        
        owner.Move();

        CheckTrace();
        CheckIdle();        
    }
    public override void Exit()
    {
        
    }

    void SetMoveTime()
    {
        if (maxMoveTime == 0)
        {
            maxMoveTime = Random.Range(3, 7);
        }
    }

    void CheckIdle()
    {
        if (currentMoveTime >= maxMoveTime)
        {
            owner.ChangeState(CaveManState.CIdle);
        }
    }

    void CheckTrace()
    {
        float dist = Vector2.Distance(owner.transform.position, owner.TargetPlayer.transform.position);

        if (CheckFrontPlayer() == true && dist < owner.DetectRange)
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
