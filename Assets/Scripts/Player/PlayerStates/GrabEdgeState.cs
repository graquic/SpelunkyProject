using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapEdgeState : StateBase<Player>
{
    public GrapEdgeState(Player owner) : base(owner)
    {
    }

    float jumpYdist = 3f;

    public override void Enter()
    {

    }

    public override void Exit()
    {
        owner.isGrabEdge = false;
        // owner.boxcol.isTrigger = false;
    }

    public override void Update()
    {
        CheckFall();
        CheckJump();
    }

    void CheckJump()
    {
        if (owner.isFlipped == true)
        {
            if (Input.GetAxis("Horizontal") > 0 && Input.GetButtonDown("Jump"))
            {
                Vector3 dir = new Vector3(owner.transform.position.x + owner.jumpXDist, owner.transform.position.y + jumpYdist) - owner.transform.position;
                dir = dir.normalized;
                owner.Rb.AddForce(dir * owner.jumpXDist, ForceMode2D.Impulse);
                owner.ChangeState(PlayerState.Jump);
            }
        }

        else
        {
            if (Input.GetAxis("Horizontal") < 0 && Input.GetButtonDown("Jump"))
            {
                Vector3 dir = new Vector3(owner.transform.position.x - owner.jumpXDist, owner.transform.position.y + jumpYdist) - owner.transform.position;
                dir = dir.normalized;
                owner.Rb.AddForce(dir * owner.jumpXDist, ForceMode2D.Impulse);
                owner.ChangeState(PlayerState.Jump);
            }
        }
    }

    void CheckFall()
    {
        if (owner.Rb.velocity.y < 0)
        {
            owner.ChangeState(PlayerState.Fall);
        }

        if (Input.GetAxisRaw("Vertical") < 0 && Input.GetButtonDown("Jump"))
        {
            // owner.checkGrabEdge.GetComponent<BoxCollider2D>().isTrigger = true; // TODO
            owner.ChangeState(PlayerState.Fall);
        }
    }
}
