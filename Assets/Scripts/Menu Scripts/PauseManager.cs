using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    private float prevSpeed;
    public static bool paused = false;
    public GameObject pauseMenu;
    private Button[] allButtons;

    public void Start()
    {
        allButtons = FindObjectsOfType<Button>();
    }

    public void Pause()
    {
        prevSpeed = Time.timeScale;
        paused = true;
        Time.timeScale = 0;
        foreach (Button button in allButtons)
        {
            button.interactable = false;
        }
        pauseMenu.SetActive(true);
    }

    public void Resume()
    {
        paused = false;
        Time.timeScale = prevSpeed;
        foreach (Button button in allButtons)
        {
            button.interactable = true;
        }
        pauseMenu.SetActive(false);
    }

    public void Menu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Start Menu");
    }

    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("UI Scene");
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (LevelManager.instance.gameOver)
            {
                return;
            }
            if (paused)
            {
                Resume();
            }
            else
            {
                StudentManager.placing = false;
                Pause();
            }
        }
    }
}
