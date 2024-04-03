using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private float prevSpeed;
    public static bool paused = false;
    public void Pause()
    {
        prevSpeed = Time.timeScale;
        paused = true;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        paused = false;
        Time.timeScale = prevSpeed;
    }

    public void Menu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Start Menu");
    }
}
