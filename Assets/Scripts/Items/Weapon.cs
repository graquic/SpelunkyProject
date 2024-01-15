using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Item
{
    protected override void Update()
    {
        if (rb.velocity.magnitude > 8)
        {
            gameObject.layer = LayerMask.NameToLayer("Item");
        }

        else if (rb.velocity.magnitude <= 8)
        {
            gameObject.layer = LayerMask.NameToLayer("IgnorePlayer");
        }
    }
}
