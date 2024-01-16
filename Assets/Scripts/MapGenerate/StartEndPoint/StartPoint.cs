using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return null;

        transform.position = GameManager.Instance.startPoint;
    }
}
