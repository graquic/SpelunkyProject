using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    Whip,
    Shot,

}


public class AttackState : StateBase<Player>
{
    Item curHoldItem;
    public AttackState(Player owner) : base(owner)
    {
    }

    public override void Enter()
    {
        curHoldItem = owner.inven.CurrentHoldItem;

        CheckAttackType();
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        // switch문 대신 if문을 사용하여 구현하기
        if(owner.CheckCurrentAnimationEnd())
        {
            CheckIdle();
            CheckMove();
            CheckFall();
        }
        

    }

    void CheckAttackType()
    {
        if(curHoldItem == null)
        {
            owner.ChangeAnimation(AttackType.Whip);
            Debug.Log("Whip");
        }

        else if(curHoldItem is Gun gun)
        {
            owner.ChangeAnimation(AttackType.Shot);
            Debug.Log("Gun");
            gun.Shoot();
        }

    }

    void CheckIdle()
    {
        if (Mathf.Abs(owner.Rb.velocity.x) == 0 && Mathf.Abs(owner.Rb.velocity.y) == 0) // move -> idle 과는 y값도 확인한다는 점에서 다름
        {
            owner.ChangeState(PlayerState.Idle);
        }
    }

    void CheckMove()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 && owner.Rb.velocity.y == 0 && owner.isGrounded)
        {
            owner.ChangeState(PlayerState.Move);
        }
    }
    void CheckFall()
    {
        if (owner.Rb.velocity.y < -0.1f)
        {
            owner.ChangeState(PlayerState.Fall);
        }
    }

}
