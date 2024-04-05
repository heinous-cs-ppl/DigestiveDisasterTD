using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    private float prevSpeed;
    public static bool paused = false;
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
    }

    public void Resume()
    {
        paused = false;
        Time.timeScale = prevSpeed;
        foreach (Button button in allButtons)
        {
            button.interactable = true;
        }
    }

    public void Menu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Start Menu");
    }
}
