using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return null;

        transform.position = GameManager.Instance.endPoint;
    }
}
