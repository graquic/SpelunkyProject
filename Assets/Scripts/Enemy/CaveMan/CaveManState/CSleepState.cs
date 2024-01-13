using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSleepState : StateBase<CaveMan>
{
    public CSleepState(CaveMan owner) : base(owner)
    {
    }

    bool isPlayed;

    public override void Enter()
    {
        owner.ChangeAnimation(CaveManState.CSleep);
    }
    public override void Update()
    {
        CheckPlayerInDetectRange();

        if(owner.CheckCurrentAnimationWait())
        {
            owner.ChangeState(CaveManState.CIdle);
        }

    }
    public override void Exit()
    {
        isPlayed = false;
    }
    void CheckPlayerInDetectRange()
    {
        if(isPlayed == false)
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(owner.transform.position, owner.DetectRange);

            if (cols.Length > 0 )
            {
                foreach(Collider2D col in cols)
                {
                    if(col.tag == "Player")
                    {
                        owner.ChangeAnimation("CAwake");
                        isPlayed = true;
                    }
                }
                
            }
        }
        
    }
}
