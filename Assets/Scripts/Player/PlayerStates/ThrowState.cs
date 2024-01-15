using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public enum ThrowType
{
    Bomb,
    Item,
    None,
}
public class ThrowState : StateBase<Player>
{
    Item curItem;

    public ThrowState(Player owner) : base(owner)
    {
    }
    

    public override void Enter()
    {
        owner.ChangeAnimation(PlayerState.Throw);

        curItem = owner.inven.CurrentHoldItem;

        switch(owner.ThrowType)
        {
            case ThrowType.Bomb:
                ThrowBomb();
                break;
            case ThrowType.Item:
                ThrowObject(curItem);
                owner.inven.SetCurrentHoldItem(null);
                break;
        }
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        if (owner.CheckCurrentAnimationEnd())
        {
            CheckIdle();
            CheckMove();
            CheckFall();
        }
    }

    void ThrowObject(Item curItem) // 잡고 있는 폭탄을 던지는 지점
    {

        if (curItem.TryGetComponent<Bomb>(out Bomb bomb))
        {
            owner.SetHoldItemSetting(bomb, true);
        }

        curItem.transform.parent = null;
        curItem.transform.position = owner.throwPoint.transform.position;

        curItem.GetComponent<Rigidbody2D>().AddForce(((Vector3.right * owner.transform.localScale.x) + new Vector3(0, 0.5f, 0)) * owner.ThrowPower, ForceMode2D.Impulse);
        
    }

    void ThrowBomb()
    {
        if (owner.inven.GetItemCount(ItemType.Bomb) <= 0) { return; }

        GameObject bomb = ObjectPoolManager.Instance.GetObject(PoolType.Bomb);

        bomb.transform.parent = null;
        bomb.transform.position = owner.throwPoint.transform.position;

        bomb.GetComponent<Rigidbody2D>().AddForce(((Vector3.right * owner.transform.localScale.x) + new Vector3(0, 0.7f, 0)) * owner.ThrowPower, ForceMode2D.Impulse);
        owner.inven.DecreaseItemFromInven(ItemType.Bomb, 1);

    }

    void CheckIdle()
    {
        if (Mathf.Abs(owner.Rb.velocity.x) <= 0.1f && Mathf.Abs(owner.Rb.velocity.y) <= 0.1f) // move -> idle 과는 y값도 확인한다는 점에서 다름
        {
            owner.ChangeState(PlayerState.Idle);
        }
    }

    void CheckMove()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 && owner.Rb.velocity.y == 0 && owner.IsGrounded)
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
