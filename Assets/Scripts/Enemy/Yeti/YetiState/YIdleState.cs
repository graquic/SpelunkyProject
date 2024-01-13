using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YIdleState : StateBase<Yeti>
{
    public YIdleState(Yeti owner) : base(owner)
    {
    }

    float maxWaitToMoveTime;
    float currentWaitToMoveTime;

    public override void Enter()
    {
        owner.ChangeAnimation(YetiState.YIdle);

        maxWaitToMoveTime = 0;
        currentWaitToMoveTime = 0;
    }

    public override void Update()
    {
        currentWaitToMoveTime += Time.deltaTime;

        if (maxWaitToMoveTime == 0)
        {
            maxWaitToMoveTime = Random.Range(2, 5);
        }

        if(currentWaitToMoveTime >= maxWaitToMoveTime)
        {
            owner.ChangeState(YetiState.YMove);
        }


    }

    public override void Exit()
    {
        maxWaitToMoveTime = 0;
        currentWaitToMoveTime = 0;
    }
}
