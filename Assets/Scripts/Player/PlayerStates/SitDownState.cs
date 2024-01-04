using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitDownState : StateBase<Player>
{
    public SitDownState(Player owner) : base(owner)
    {
    }

    private float currentWaitTime;
    private float maxWaitTime = 1f;

    public override void Enter()
    {
        currentWaitTime = 0;
        owner.transform.Find("Sprite").transform.position -= new Vector3(0, 0.3f, 0);
    }

    public override void Exit()
    {
        currentWaitTime = 0;
        
    }

    public override void Update()
    {
        CoordinateImage();

        CheckSitUp();
        CheckFall();

        LookDown();
    }
    void CoordinateImage()
    {
        
        
    }


    void CheckSitUp()
    {
        if (Input.GetAxisRaw("Vertical") >= 0)
        {
            owner.ChangeState(PlayerState.SitUp);
        }
    }

    void CheckFall()
    {
        if (owner.rb.velocity.y < -0.1f)
        {
            owner.ChangeState(PlayerState.Fall);
        }
    }

    void LookDown()
    {
        currentWaitTime += Time.deltaTime;

        if (currentWaitTime > maxWaitTime)
        {
            owner.camController.LookDown();
        }
    }

}
