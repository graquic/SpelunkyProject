using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckOnTheEdge : MonoBehaviour
{
    [SerializeField] Player player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Ground" && player.rb.velocity.y == 0)
        {
            player.isOnTheEdge = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Ground" && player.rb.velocity.y == 0)
        {
            player.isOnTheEdge = true;
        }
    }


}
