using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] GameStatusUI gameStatusPanel;
    public GameStatusUI GameStatusPanel { get { return gameStatusPanel; } }
    [SerializeField] GameOverPanelUI gameOverPanel;
    public GameOverPanelUI GameOverPanel { get { return gameOverPanel; } }
    [SerializeField] PausedPanelUI pausedPanel;
    public PausedPanelUI PausedPanel { get { return pausedPanel; } }

    [SerializeField] float waitTimeForUI;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
        // DontDestroyOnLoad(gameObject);

        InitializeUIRef();
    }

    private void Update()
    {
        if(pausedPanel == null)
        {
            pausedPanel = FindObjectOfType<PausedPanelUI>();
        }


        if (pausedPanel.gameObject.activeSelf == false && Input.GetKeyDown(KeyCode.Escape))
        {
            pausedPanel.gameObject.SetActive(true);
        }
    }

    public void InitializeUIRef()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if(currentSceneName == "StageScene")
        {
            if (gameStatusPanel == null)
            {
                gameStatusPanel = FindObjectOfType<GameStatusUI>();
            }

            if (gameOverPanel == null)
            {
                gameOverPanel = FindObjectOfType<GameOverPanelUI>();
            }

            if (pausedPanel == null)
            {
                pausedPanel = FindObjectOfType<PausedPanelUI>();
            }
        }
        
    }

    public void ControlRefUI(string SceneName)
    {
        if(SceneName == "MainMenuScene")
        {
            gameStatusPanel.gameObject.SetActive(false);
            gameOverPanel.gameObject.SetActive(false);
            pausedPanel.gameObject.SetActive(false);
        }
        else if(SceneName == "StageScene")
        {
            gameStatusPanel.gameObject.SetActive(true);
        }
        
    }

    public void EnableGameOverUI()
    {
        StartCoroutine(SetOnGameOverUI());
    }

    IEnumerator SetOnGameOverUI()
    {
        yield return new WaitForSeconds(waitTimeForUI);

        gameOverPanel.gameObject.SetActive(true);
    }
}
