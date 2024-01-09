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
    

    private float currentWaitTime;
    private float maxWaitTime = 1f;

    public override void Enter()
    {
        currentWaitTime = 0;        
        owner.transform.Find("Sprite").transform.position -= new Vector3(0, 0.3f, 0);

        // SetBoxCol();

    }

    public override void Exit()
    {
        currentWaitTime = 0;
        owner.transform.Find("Sprite").transform.position += new Vector3(0, 0.3f, 0);
    }

    public override void Update()
    {
        CheckHoldItem();
        CheckSitUp();
        CheckFall();

        LookDown();
        DropBomb();
    }

    void CheckHoldItem()
    {
        if(owner.inven.currentHoldItem == null && Input.GetButtonDown("Attack"))
        {
            float minDist = 999;

            Vector2 BL = new Vector2(owner.transform.position.x - 0.6f, owner.transform.position.y - 0.6f);
            Vector2 TR = new Vector2(owner.transform.position.x + 0.7f, owner.transform.position.y - 0.2f);


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
                        owner.inven.currentHoldItem = item;
                    }
                }
            }

            if (owner.inven.currentHoldItem != null)
            {
                owner.inven.currentHoldItem.gameObject.layer = LayerMask.NameToLayer("Item");
                owner.inven.currentHoldItem.transform.parent = owner.hand;
                owner.inven.currentHoldItem.transform.localPosition = new Vector2(0.3f, -0.3f);
                owner.inven.currentHoldItem.GetComponent<Collider2D>().isTrigger = true;
                owner.inven.currentHoldItem.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                owner.inven.currentHoldItem.GetComponent<Rigidbody2D>().gravityScale = 0;
            }

        }

        else if (owner.inven.currentHoldItem != null && Input.GetButtonDown("Attack"))
        {
            owner.inven.currentHoldItem.gameObject.layer = LayerMask.NameToLayer("IgnorePlayer");
            owner.inven.currentHoldItem.transform.parent = null;
            owner.inven.currentHoldItem.GetComponent<Collider2D>().isTrigger = false;
            owner.inven.currentHoldItem.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            owner.inven.currentHoldItem.GetComponent<Rigidbody2D>().gravityScale = 1;
            owner.inven.currentHoldItem = null;
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
