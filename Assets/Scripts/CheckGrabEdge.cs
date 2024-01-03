using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGrabEdge : MonoBehaviour
{
    [SerializeField] Player player;

    private void Update()
    {
        transform.localPosition = Vector3.zero + new Vector3(0.5f, 0.17f, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground")
        {
            player.isGrabEdge = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Ground" || collision.tag == "Wall")
        {
            player.isGrabEdge = false;
        }
    }
}
