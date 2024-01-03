using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEdge : MonoBehaviour
{
    [SerializeField] Player player;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground")
        {
            player.isGrabEdge = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            player.isGrabEdge = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Ground")
        {
            player.isOnTheEdge = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            player.isOnTheEdge = true;
        }
    }
}
