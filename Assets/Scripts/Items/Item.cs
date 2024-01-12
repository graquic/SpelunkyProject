using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Gun,
    Bomb,
    Rope,
    Fuel,
}

[RequireComponent(typeof(Rigidbody2D))]
public class Item : MonoBehaviour , IHoldable
{
    [HideInInspector] public Rigidbody2D rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        
        if (rb.velocity.magnitude > 10)
        {
            gameObject.layer = LayerMask.NameToLayer("Item");
        }

        else if (rb.velocity.magnitude <= 10)
        {
            gameObject.layer = LayerMask.NameToLayer("IgnorePlayer");
        }
    }

    protected virtual void OnDisable()
    {
        if (GameManager.Instance.player.inven.CurrentHoldItem == this)
        {
            GameManager.Instance.player.inven.SetCurrentHoldItem(null);
        }
    }

    public void SetHoldItem(Player player)
    {
        
    }
}
