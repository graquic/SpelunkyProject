using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundReposition : MonoBehaviour
{
    Player player;

    private IEnumerator Start()
    {
        player = GameManager.Instance.player;

        yield return null;

        yield return null;

        transform.position += player.transform.position;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("BackgroundArea") == false) return;

        Vector3 playerPos = GameManager.Instance.player.transform.position;
        Vector3 thisPos = transform.position;

        float diffX = Mathf.Abs(playerPos.x - thisPos.x);
        float diffY = Mathf.Abs(playerPos.y - thisPos.y);

        float dirX = playerPos.x > thisPos.x ? 1 : -1;
        float dirY = playerPos.y > thisPos.y ? 1 : -1;
        
        if (diffX > diffY)
        {
            transform.Translate(Vector3.right * dirX * 60);
        }

        else if (diffX < diffY)
        {
            transform.Translate(Vector3.up * dirY * 60);
        }
        
    }
}
