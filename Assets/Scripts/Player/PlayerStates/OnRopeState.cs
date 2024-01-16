using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OnRopeState : StateBase<Player>
{
    public OnRopeState(Player owner) : base(owner)
    {
    }

    private enum OnRopeType
    {
        OnRopeIdle,
        OnRopeMove,
    }

    Transform touchedRope;

    OnRopeType curState;
    float ropeSpeed = 5f;

    public override void Enter()
    {
        curState = OnRopeType.OnRopeIdle;
        owner.isOnRope = true;

        touchedRope = owner.touchedRope.transform;
        owner.transform.position = new Vector3(touchedRope.position.x, owner.transform.position.y, 0);

        owner.ChangeAnimation("OnRopeIdle");
        owner.Rb.gravityScale = 0f;
        owner.Rb.velocity = Vector3.zero;
    }
    public override void Update()
    {
        
        switch(curState)
        {
            case OnRopeType.OnRopeIdle:
                owner.Rb.velocity = Vector2.zero;

                if (Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0)
                {
                    curState = OnRopeType.OnRopeMove;
                    owner.ChangeAnimation("OnRopeMove");
                    break;
                }
                CheckJump();
                CheckRope();
                break;

            case OnRopeType.OnRopeMove:
                if(Mathf.Abs(Input.GetAxisRaw("Vertical")) == 0)
                {
                    curState = OnRopeType.OnRopeIdle;
                    owner.ChangeAnimation("OnRopeIdle");
                    break;
                }
                CheckJump();
                CheckRope();
                owner.Rb.velocity = new Vector2(0, Input.GetAxisRaw("Vertical") * ropeSpeed);
               
                break;
        }

    }

    public override void Exit()
    {
        owner.Rb.gravityScale = 1f;
        owner.isOnRope = false;
        owner.touchedRope = null;
    }

    void CheckJump()
    {
        if(Input.GetAxisRaw("Horizontal") > 0 && Input.GetButtonDown("Jump"))
        {
            owner.Rb.AddForce(Vector2.right * owner.JumpPowerY * 2);
            owner.ChangeState(PlayerState.Jump);
        }

        else if(Input.GetAxisRaw("Horizontal") < 0 && Input.GetButtonDown("Jump"))
        {
            owner.Rb.AddForce(Vector2.left * owner.JumpPowerY * 2);
            owner.ChangeState(PlayerState.Jump);
        }

        
    }

    void CheckRope()
    {
        if (owner.inven.GetItemCount(ItemType.Rope) > 0 && Input.GetButtonDown("Rope"))
        {
            GameObject rope = ObjectPoolManager.Instance.GetObject(PoolType.Rope);
            rope.transform.parent = null;
            rope.transform.position = owner.transform.position + new Vector3(0, 0.5f, 0);

            owner.inven.DecreaseItemFromInven(ItemType.Rope, 1);
        }
    }
}
