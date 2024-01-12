using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory :MonoBehaviour
{
    private Dictionary<ItemType, List<Item>> ownItems;

    private Player player;
    private Item currentHoldItem;
    public Item CurrentHoldItem { get { return currentHoldItem; } }


    private void Awake()
    {
        player = GetComponent<Player>();
    }

    public void AddItem(ItemType type, Item item)
    {
        if (ownItems[type] == null)
        {
            ownItems[type] = new List<Item>();
            ownItems[type].Add(item);
        }

        else
        {
            ownItems[type].Add(item);
        }    
    }

    public void SetCurrentHoldItem(Item item)
    {
        currentHoldItem = item;
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
