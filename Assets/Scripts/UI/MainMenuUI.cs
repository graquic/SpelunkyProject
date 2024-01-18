using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Button startButton;

    private void Awake()
    {
        
    }

    public void LoadStageScene()
    {
        SceneManager.LoadScene("StageScene");
    }
}
