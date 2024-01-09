using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowState : StateBase<Player>
{
    Item curItem;

    public ThrowState(Player owner) : base(owner)
    {
    }

    public override void Enter()
    {
        curItem = owner.inven.currentHoldItem;

        switch(owner.throwType)
        {
            case ThrowType.Bomb:
                ThrowBomb();
                break;
            case ThrowType.Item:
                ThrowItem();
                break;
        }

        owner.ChangeState(PlayerState.Idle);
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        
    }

    void ThrowBomb()
    {
        GameObject obj = ObjectPoolManager.Instance.GetObject(PoolType.Bomb);
        obj.transform.parent = null;
        obj.transform.position = owner.throwPoint.transform.position;

        Vector3 dir = obj.transform.TransformDirection(Vector3.right);
        obj.GetComponent<Rigidbody2D>().AddForce(dir * owner.transform.localScale.x * owner.ThrowPower, ForceMode2D.Impulse);
    }


    void ThrowItem()
    {
        curItem.rb.AddForce(Vector2.right * owner.ThrowPower, ForceMode2D.Impulse);
    }
}
