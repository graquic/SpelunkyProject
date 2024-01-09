using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateBase<Player>
{
    Item curHoldItem;
    public AttackState(Player owner) : base(owner)
    {
    }

    public override void Enter()
    {
        curHoldItem = owner.inven.currentHoldItem;
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        // switch�� ��� if���� ����Ͽ� �����ϱ�

        
        if (owner.CheckCurrentAnimationEnd())
        {
            owner.ChangeState(PlayerState.Idle);
        }
        
    }

    void CheckItemToAction()
    {

    }

    void Throw()
    {

    }

    void Attack()
    {

    }
}
