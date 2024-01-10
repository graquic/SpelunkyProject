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
        Debug.Log(2);
        curItem = owner.inven.CurrentHoldItem;

        switch(owner.throwType)
        {
            case ThrowType.Bomb:
                GameObject bomb = ObjectPoolManager.Instance.GetObject(PoolType.Bomb);
                ThrowObject(bomb);
                break;
            case ThrowType.Item:
                ThrowObject(curItem.gameObject);
                GameManager.Instance.player.inven.SetCurrentHoldItem(null);
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
            CheckIdle();
            CheckMove();
            CheckFall();
        }
    }

    void ThrowObject(GameObject obj)
    {
        obj.transform.parent = null;
        obj.transform.position = owner.throwPoint.transform.position;

        obj.GetComponent<Rigidbody2D>().AddForce(Vector3.right * owner.transform.localScale.x * owner.ThrowPower, ForceMode2D.Impulse);
        
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
