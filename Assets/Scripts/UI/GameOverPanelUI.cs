using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanelUI : MonoBehaviour
{
    [SerializeField] Toggle[] selectToggles;

    [SerializeField] Sprite isOnImage;
    [SerializeField] Sprite isOffImage;

    int prevIdx;
    int curIdx;

    private void Awake()
    {
        curIdx = 0;
    }

    private void Update()
    {
        CheckInput();
    }

    void CheckInput()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            curIdx++;
        }

        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            curIdx--;
        }
        if (curIdx < 0) { curIdx = selectToggles.Length - 1; }
        curIdx = curIdx % selectToggles.Length;

        
    }

    void SetToggles(int idx, bool isOn)
    {
        selectToggles[idx].isOn = isOn;

        if (isOn == true)
        {
            selectToggles[idx].GetComponentInChildren<Image>().sprite = isOnImage;
            selectToggles[idx].GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
        }
        else
        {
            selectToggles[idx].GetComponentInChildren<Image>().sprite = isOffImage;
            selectToggles[idx].GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }

    }
}
