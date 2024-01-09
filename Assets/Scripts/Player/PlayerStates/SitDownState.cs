using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SitDownState : StateBase<Player>
{
    public SitDownState(Player owner) : base(owner)
    {
    }
    Item curHoldItem;

    private float currentWaitTime;
    private float maxWaitTime = 1f;

    public override void Enter()
    {
        currentWaitTime = 0;        
        owner.transform.Find("Sprite").transform.position -= new Vector3(0, 0.3f, 0);

        curHoldItem = owner.inven.currentHoldItem;
        // SetBoxCol();

    }

    public override void Exit()
    {
        currentWaitTime = 0;
        owner.inven.currentHoldItem = curHoldItem;
    }

    public override void Update()
    {
        CheckHoldItem();
        CheckSitUp();
        CheckFall();

        LookDown();
        DropBomb();
    }

    void SetBoxCol()
    {
        owner.triggeredCol.offset = new Vector2(-0.04287338f, -0.3467688f);
        owner.triggeredCol.size = new Vector2(1.088626f, 0.2080814f);
    }

    void CheckHoldItem()
    {
        if(curHoldItem == null && Input.GetButtonDown("Attack"))
        {
            float minDist = 999;

            Vector2 BL = new Vector2(owner.transform.position.x - 0.5f, owner.transform.position.y - 0.6f);
            Vector2 TR = new Vector2(owner.transform.position.x + 0.6f, owner.transform.position.y - 0.25f);


            Collider2D[] cols = Physics2D.OverlapAreaAll(BL, TR);

            if (cols == null) { return; }

            foreach(Collider2D col in cols)
            {
                if (col.gameObject.TryGetComponent<Item>(out Item item))
                {
                    float dist = Vector2.Distance(col.transform.position, owner.transform.position);

                    if (minDist > dist)
                    {
                        minDist = dist;
                        curHoldItem = item;
                    }
                }
            }

            if (curHoldItem != null)
            {
                curHoldItem.transform.parent = owner.hand;
                curHoldItem.transform.localPosition = new Vector2(0.3f, -0.3f);

                Debug.Log(curHoldItem);
                Debug.Log(owner.inven.currentHoldItem);
            }

            else { Debug.Log("아이템이 없음"); }            
        }

        else if (curHoldItem != null && Input.GetButtonDown("Attack"))
        {
            curHoldItem.transform.parent = null;
            curHoldItem = null;
        }
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
