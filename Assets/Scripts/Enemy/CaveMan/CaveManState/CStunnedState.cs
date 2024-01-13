using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;

public class CStunnedState : StateBase<CaveMan>
{
    public CStunnedState(CaveMan owner) : base(owner)
    {
    }

    private enum CStunnedType
    {
        Pushed, CHitFall, CStunned,
    }

    CStunnedType curState;
    int target = LayerMask.GetMask("Ground");

    public override void Enter()
    {
        owner.SetIgnorePlayer(true);

        CheckStunnedPose();
        CollideWallBounce();
    }
    public override void Update()
    {
        switch (curState)
        {
            case CStunnedType.Pushed:

                if (owner.Rb.velocity.y < -0.3f)
                {
                    curState = CStunnedType.CHitFall;
                    owner.ChangeAnimation("CHitFall");
                }

                else if (owner.BodyCol.IsTouchingLayers(target) == true && Mathf.Abs(owner.Rb.velocity.y) < 0.1f)
                {
                    curState = CStunnedType.CStunned;
                    owner.ChangeAnimation("CStunned");
                }
                break;

            case CStunnedType.CHitFall:
                if(owner.BodyCol.IsTouchingLayers(target) && Mathf.Abs(owner.Rb.velocity.y) < 0.1f)
                {
                    curState = CStunnedType.CStunned;
                    owner.ChangeAnimation("CStunned");
                }
                break;

            case CStunnedType.CStunned:
                if(owner.CheckCurrentAnimationEnd())
                {
                    owner.ChangeState(CaveManState.CIdle);
                }
                break;
        }
    }
    public override void Exit()
    {
        owner.SetIgnorePlayer(false);
    }

    void CheckStunnedPose()
    {
        if (owner.transform.localScale.x > 0)
        {
            owner.ChangeAnimation("CFrontHit");
        }

        else if (owner.transform.localScale.x < 0)
        {
            owner.ChangeAnimation("CBackHit");
        }

        curState = CStunnedType.Pushed;
    }

    void CollideWallBounce()
    {
        if(owner.IsCollideWall == true)
        {
            int dir = -(int)owner.transform.localScale.x;

            float dirX = dir * owner.PushedDir.x;
            Vector2 bounceDir = new Vector2(dirX, owner.PushedDir.y).normalized;
            owner.PushBack(bounceDir , owner.PushPower);

            owner.SetIsCollideWall(false);
        }
        
        
    }

}
