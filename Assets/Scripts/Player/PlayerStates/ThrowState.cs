using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
                GameObject bomb = ObjectPoolManager.Instance.GetObject(PoolType.Bomb);
                ThrowObject(bomb);
                break;
            case ThrowType.Item:
                ThrowObject(curItem.gameObject);
                GameManager.Instance.player.inven.currentHoldItem = null;
                break;
        }
    }

    public override void Exit()
    {
        owner.throwType = ThrowType.None;
    }

    public override void Update()
    {
        if (owner.CheckCurrentAnimationEnd())
        {
            owner.ChangeState(PlayerState.Idle);
        }
    }

    void ThrowObject(GameObject obj)
    {
        obj.transform.parent = null;
        obj.transform.position = owner.throwPoint.transform.position;

        obj.GetComponent<Rigidbody2D>().AddForce(Vector3.right * owner.transform.localScale.x * owner.ThrowPower, ForceMode2D.Impulse);
    }

}
