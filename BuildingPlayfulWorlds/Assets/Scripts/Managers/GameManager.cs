using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool gameStopped;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) && gameStopped)
        {
            RestartGame();
        }
    }

    public void GameOver()
    {
        UIManager.Instance.OpenGameOverCanvas();
        gameStopped = true;
        Time.timeScale = 0;
    }

    public void Victory()
    {
        UIManager.Instance.OpenVictoryCanvas();
        gameStopped = true;
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        gameStopped = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
