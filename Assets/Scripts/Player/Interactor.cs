using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactor : MonoBehaviour
{
    public UnityEvent OnChange;
    Player player;

    private void Start()
    {
        player = GameManager.Instance.player;  
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Bomb>(out Bomb bomb))
        {
            bomb.GetComponent<CircleCollider2D>().isTrigger = true;
            bomb.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Bomb>(out Bomb bomb))
        {
            bomb.GetComponent<CircleCollider2D>().isTrigger = false;
            bomb.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
    }
    */
}
