using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Button startButton;

    private void Awake()
    {
        /*
        if(startButton == null)
        {
            Transform start = transform.Find("StartButton");
            startButton = start.GetComponent<Button>();
        }
        */
    }
}
