using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory :MonoBehaviour
{
    private Dictionary<ItemType, int> ownItems = new();

    private Player player;
    private Item currentHoldItem;
    public Item CurrentHoldItem { get { return currentHoldItem; } }

    public UnityEvent ChangedBombCount;
    public UnityEvent ChangedRopeCount;


    private void Awake()
    {
        player = GetComponent<Player>();

        ownItems[ItemType.Bomb] = 10;
        ownItems[ItemType.Rope] = 10;
    }

    public void AddItemToInven(ItemType type, int count)
    {
        ownItems[type] += count;
    }
    
    public void DecreaseItemFromInven(ItemType type, int count)
    {
        ownItems[type] -= count;

        if (type == ItemType.Bomb)
        {
            ChangedBombCount.Invoke();
        }

        else if (type == ItemType.Rope)
        {
            ChangedRopeCount.Invoke();
        }
    }

    public int GetItemCount(ItemType type)
    {
        return ownItems[type];
    }
    

    public void SetCurrentHoldItem(Item item)
    {
        currentHoldItem = item;

        /*
        if (item != null)
        {
            currentHoldItem = item;
            currentHoldItem.transform.parent = player.hand;
        }

        else
        {
            currentHoldItem = null;
            currentHoldItem.transform.parent = null;
        }
        */
    }

    public Item FindClosestObject()
    {
        Item closestItem = null;
        float minDist = 999;

        Vector2 BL = new Vector2(player.transform.position.x - 0.6f, player.transform.position.y - 0.6f);
        Vector2 TR = new Vector2(player.transform.position.x + 0.7f, player.transform.position.y - 0.2f);

        Collider2D[] cols = Physics2D.OverlapAreaAll(BL, TR);
        if (cols == null) { return null; }

        foreach (Collider2D col in cols)
        {
            if (col.gameObject.TryGetComponent<Item>(out Item item))
            {
                float dist = Vector2.Distance(col.transform.position, player.transform.position);

                if (minDist > dist)
                {
                    minDist = dist;
                    closestItem = item;
                }
            }
            /*
            else if(col.gameObject.TryGetComponent<Yeti>(out Yeti yeti) && yeti.CurState == YetiState.YDeath)
            {
                float dist = Vector2.Distance(col.transform.position, player.transform.position);

                if (minDist > dist)
                {
                    minDist = dist;
                    closestItem = yeti;
                }
            }
            */
        }

        return closestItem;
    }
}
