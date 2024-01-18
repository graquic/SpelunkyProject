using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PausedPanelUI : MonoBehaviour
{
    GameStatusUI gameStatusPanel;

    [SerializeField] Sprite isOnImage;
    [SerializeField] Sprite isOffImage;

    [SerializeField] Toggle[] toggles;

    Animator animator;
    [SerializeField] AnimationClip[] animations;

    private bool isOnPaused;
    public bool IsOnPaused { get { return isOnPaused; } }

    int prevIdx;
    int currentIdx;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        gameStatusPanel = UIManager.Instance.GameStatusPanel;
    }
    private void OnEnable()
    {
        StartCoroutine(SetAnimationWithPause("Start"));
        GameManager.Instance.player.enabled = false;
        gameStatusPanel.gameObject.SetActive(false);
    }

    private void Update()
    {
        CheckPressKey();
        CheckEnterToggles(currentIdx);
        
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("End") 
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            gameObject.SetActive(false);
        }
        
    }

    private void OnDisable()
    {
        foreach(var toggle in toggles)
        {
            toggle.isOn = false;
        }
    }
    
    IEnumerator SetAnimationWithPause(string animName)
    {
        
        if(animName == "Start")
        {
            animator.Play(animName);

            float startLength = animations[0].length;

            yield return new WaitForSeconds(startLength);

            Time.timeScale = 0;

        }

        else if (animName == "End")
        {
            Time.timeScale = 1;

            animator.Play(animName);
        }
    }


    void CheckPressKey()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            prevIdx = currentIdx;
            currentIdx++;
        }

        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            prevIdx = currentIdx;
            currentIdx--;
        }

        if (currentIdx < 0) { currentIdx = toggles.Length - 1; }
        currentIdx = currentIdx % toggles.Length;

        SetToggles(prevIdx, false);
        SetToggles(currentIdx, true);
    }

    void CheckEnterToggles(int idx)
    {
        if(Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetButtonDown("Attack"))
        {
            GameManager.Instance.player.enabled = true;
            gameStatusPanel.gameObject.SetActive(true);

            if (idx == 0)
            {
                StartCoroutine(SetAnimationWithPause("End"));
            }

            else if (idx == 1)
            {
                Time.timeScale = 1;
                EnemySpawnManager.Instance.ResetEnemyList();
                SceneController.Instance.LoadScene("MainMenuScene");
            }

            else if (idx == 2)
            {
                // 시간 남으면 정말로 게임을 종료할건지 추가 질문 UI 만들기
                Application.Quit();
            }
        }

        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(SetAnimationWithPause("End"));
        }
        
    }

    void SetToggles(int idx, bool isOn)
    {
        toggles[idx].isOn = isOn;

        if (isOn == true)
        {
            toggles[idx].GetComponentInChildren<Image>().sprite = isOnImage;
            toggles[idx].GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
        }
        else
        {
            toggles[idx].GetComponentInChildren<Image>().sprite = isOffImage;
            toggles[idx].GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }
        
    }
}
