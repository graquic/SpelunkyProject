using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YMoveState : StateBase<Yeti>
{
    public YMoveState(Yeti owner) : base(owner)
    {
    }

    float maxMoveTime;
    float currentMoveTime;
    public override void Enter()
    {
        owner.ChangeAnimation(YetiState.YMove);

        maxMoveTime = 0;
        currentMoveTime = 0;

        owner.SetMoveDirection();
    }

    public override void Update()
    {
        owner.Move();

        currentMoveTime += Time.deltaTime;

        if (maxMoveTime == 0)
        {
            maxMoveTime = Random.Range(1, 3);
        }

        if(currentMoveTime >= maxMoveTime)
        {
            owner.ChangeState(YetiState.YIdle);
        }
    }

    public override void Exit()
    {
        maxMoveTime = 0;
        currentMoveTime = 0;
    }

    
}
