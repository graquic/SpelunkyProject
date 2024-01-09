using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckOnTheEdge : MonoBehaviour
{
    Player player;

    private void Start()
    {
        player = GameManager.Instance.player;
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
        if(collision.tag == "Ground")
        {
            player.isOnTheEdge = true;
        }
    }


}
