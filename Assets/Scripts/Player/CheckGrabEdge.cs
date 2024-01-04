using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGrabEdge : MonoBehaviour
{
    [SerializeField] Player player;

    public Collider2D grapper;

    private void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground")
        {
            player.isGrabEdge = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.tag == "Edge")
        {
            grapper.enabled = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Ground" || collision.tag == "Wall")
        {
            player.isGrabEdge = false;
            grapper.enabled = false;

        }
    }
}
