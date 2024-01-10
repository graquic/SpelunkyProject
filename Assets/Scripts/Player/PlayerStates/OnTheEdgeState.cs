using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTheEdgeState : IdleState
{
    public OnTheEdgeState(Player owner) : base(owner)
    {
        
    }

    public override void Enter()
    {
        owner.ChangeAnimation(PlayerState.OnTheEdge);
    }

}
