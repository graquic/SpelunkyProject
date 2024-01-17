using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas : MonoBehaviour
{
    Canvas canvas;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
