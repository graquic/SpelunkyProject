using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerceiveWall : MonoBehaviour
{
    Transform parentTransform;

    private void Awake()
    {
        if(transform.parent != null)
        {
            parentTransform = transform.parent;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Ground" || collision.tag == "Wall")
        {
            float currentX = parentTransform.localScale.x;

            parentTransform.localScale = new Vector3(-currentX, parentTransform.localScale.y, parentTransform.localScale.z);
        }
    }
}
