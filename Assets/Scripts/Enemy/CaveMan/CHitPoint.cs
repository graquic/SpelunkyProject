using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHitPoint : MonoBehaviour
{
    [SerializeField] CaveMan owner;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player")
        {
            Player player = collision.collider.GetComponent<Player>();

            owner.TakeDamage(player.StepDamage);

            player.Rb.AddForce(new Vector2(0, 1f), ForceMode2D.Impulse);
            player.ChangeState(PlayerState.Jump);
        }
    }
}
