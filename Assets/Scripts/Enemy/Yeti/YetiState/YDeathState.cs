using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YDeathState : StateBase<Yeti>
{
    public YDeathState(Yeti owner) : base(owner)
    {
    }

    public override void Enter()
    {
        owner.ChangeAnimation(YetiState.YDeath);

        owner.gameObject.layer = LayerMask.NameToLayer("IgnorePlayer");

        GameManager.Instance.AddCurScore(owner.Score);
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        
    }
}
