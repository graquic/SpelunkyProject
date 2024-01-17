using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanelUI : MonoBehaviour
{
    [Header("선택 toggle 리스트")]
    [SerializeField] Toggle[] selectToggles;

    [Header("결과 Text")]
    [SerializeField] TextMeshProUGUI stageLevelText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI curTimeText;

    [Header("변경 이미지")]
    [SerializeField] Sprite isOnImage;
    [SerializeField] Sprite isOffImage;

    int minute;
    int second;

    int prevIdx;
    int curIdx;

    private void Awake()
    {
        curIdx = 0;
    }

    private void OnEnable()
    {
        stageLevelText.text = $"{GameManager.Instance.WorldStageLevel} - {GameManager.Instance.SubStageLevel}";
        scoreText.text = GameManager.Instance.CurScore.ToString();

        minute = Mathf.FloorToInt(Time.time % 3600) / 60;
        second = Mathf.FloorToInt(Time.time % 60);
        curTimeText.text = $"{minute : 00} : {second : 00}";
    }

    private void Update()
    {
        CheckInput();
    }

    void CheckInput()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            prevIdx = curIdx;
            curIdx++;
        }

        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            prevIdx = curIdx;
            curIdx--;
        }
        if (curIdx < 0) { curIdx = selectToggles.Length - 1; }
        curIdx = curIdx % selectToggles.Length;

        SetToggles(prevIdx, false);
        SetToggles(curIdx, true);
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
