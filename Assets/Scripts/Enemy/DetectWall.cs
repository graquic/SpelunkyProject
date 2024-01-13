using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectWall : MonoBehaviour
{
    [SerializeField] CaveMan owner;

    private void Awake()
    {
       if(owner == null)
        {
            owner = transform.parent.GetComponent<CaveMan>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Ground" || collision.tag == "Wall")
        {
            if (owner.CurState == CaveManState.CTrace)
            {
                owner.SetIsCollideWall(true);
                owner.ChangeState(CaveManState.CStunned);
            }

            int dir = -(int) owner.transform.localScale.x;
            owner.SetMoveDirection(dir);
        }
    }
}
