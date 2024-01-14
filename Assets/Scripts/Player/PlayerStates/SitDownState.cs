using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;

public class SitDownState : StateBase<Player>
{
    public SitDownState(Player owner) : base(owner)
    {
    }
    Item curItem;

    private float currentWaitTime;
    private float maxWaitTime = 1f;

    public override void Enter()
    {
        owner.ChangeAnimation(PlayerState.SitDown);
        currentWaitTime = 0;        
        
        owner.transform.Find("Sprite").transform.position -= new Vector3(0, 0.3f, 0);
        owner.hand.transform.position -= new Vector3(0, 0.2f, 0);
        

        // SetBoxCol();

    }

    public override void Exit()
    {
        currentWaitTime = 0;

        owner.transform.Find("Sprite").transform.position += new Vector3(0, 0.3f, 0);
        owner.hand.transform.position += new Vector3(0, 0.2f, 0);
    }

    public override void Update()
    {
        CheckSitUp();
        CheckFall();

        LookDown();

        CheckDropItem();
        DropBomb();
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
        if (owner.Rb.velocity.y < -0.1f)
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

    void CheckDropItem()
    {
        if (owner.inven.CurrentHoldItem == null && Input.GetButtonDown("Attack"))
        {
            Item closestItem = owner.inven.FindClosestObject();

            if(closestItem != null) // 아이템 돌려놓기
            {
                closestItem.transform.localScale = owner.transform.localScale;

                owner.inven.SetCurrentHoldItem(closestItem);
            
                curItem = owner.inven.CurrentHoldItem;

                curItem.gameObject.layer = LayerMask.NameToLayer("Item");
                curItem.transform.parent = owner.hand;
                curItem.transform.localPosition = new Vector2(0.3f, -0.3f);

                owner.SetHoldItemSetting(curItem, false);
            }

        }

        else if (owner.inven.CurrentHoldItem != null && Input.GetButtonDown("Attack"))
        {
            curItem = owner.inven.CurrentHoldItem;
            curItem.gameObject.layer = LayerMask.NameToLayer("IgnorePlayer");
            curItem.transform.parent = null;

            owner.SetHoldItemSetting(curItem, true);

            owner.inven.SetCurrentHoldItem(null);
        }
    }

    void DropBomb()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            GameObject bomb = ObjectPoolManager.Instance.GetObject(PoolType.Bomb);
            bomb.SetActive(true);
            bomb.transform.position = owner.transform.position;
        }
    }

}
