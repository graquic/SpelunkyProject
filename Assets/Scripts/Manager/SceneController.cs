using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        // SceneManager.sceneLoaded는 기본적으로 Scene scene과 LoadSceneMode mode 라는 매개변수를 가지는 함수를 받음
    }

    public void LoadStageScene()
    {
        SceneManager.LoadScene("StageScene");
    }

    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReloadCurrentScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "MainMenuScene")
        {
            UIManager.Instance.ControlRefUI(scene.name);
        }

        else if(scene.name == "StageScene")
        {
            UIManager.Instance.InitializeUIRef();
            UIManager.Instance.ControlRefUI(scene.name);
            GameManager.Instance.SetPlayer();
        }
    }
}
