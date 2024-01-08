using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private Dictionary<ItemType, List<Item>> ownItems;

    public Item currentHoldItem;

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
}
