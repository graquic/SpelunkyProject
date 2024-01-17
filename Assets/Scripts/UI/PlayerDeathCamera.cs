using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathCamera : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.position = GameManager.Instance.player.transform.position + new Vector3(0,0,-10);
    }
}
