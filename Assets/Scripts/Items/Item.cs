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
public class Item : MonoBehaviour
{
    public Rigidbody2D rb;

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
}
