using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] GameStatusUI gameStatusPanel;
    public GameStatusUI GameStatusPanel { get { return gameStatusPanel; } }
    [SerializeField] GameOverPanelUI gameOverPanel;
    public GameOverPanelUI GameOverPanel { get { return gameOverPanel; } }
    [SerializeField] PausedPanelUI pausedPanel;
    public PausedPanelUI PausedPanel { get { return PausedPanel; } }


    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    private void Update()
    {
        if (pausedPanel.gameObject.activeSelf == false && Input.GetKeyDown(KeyCode.Escape))
        {
            pausedPanel.gameObject.SetActive(true);
        }
    }
}
