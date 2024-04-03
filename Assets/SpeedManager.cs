using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    public static SpeedManager instance;
    public UnityEngine.UI.Button slowButton;
    public UnityEngine.UI.Button speedButton;

    public KeyCode slowKey = KeyCode.Q;
    public KeyCode speedKey = KeyCode.E;

    public void Awake()
    {
        instance = this;
        Time.timeScale = 1;
    }

    public void Start()
    {
        slowButton.onClick.AddListener(ToggleSlowDown);
        speedButton.onClick.AddListener(ToggleSpeedUp);
    }

    public void Update()
    {
        if (PauseMenu.paused)
        {
            return;
        }
        if (Input.GetKeyDown(slowKey))
        {
            SlowDown();
        }
        else if (Input.GetKeyDown(speedKey))
        {
            SpeedUp();
        }
        else if (Input.GetKeyUp(slowKey))
        {
            if (Input.GetKey(speedKey))
            {
                SpeedUp();
            }
            else
            {
                NormalSpeed();
            }
        }
        else if (Input.GetKeyUp(speedKey))
        {
            if (Input.GetKey(slowKey))
            {
                SlowDown();
            }
            else
            {
                NormalSpeed();
            }
        }
    }

    public void NormalSpeed()
    {
        if (!LevelManager.instance.gameOver)
        {
            Time.timeScale = 1;
        }
    }

    public void SlowDown()
    {
        if (!LevelManager.instance.gameOver)
        {
            Time.timeScale = 0.5f;
        }
    }

    public void SpeedUp()
    {
        if (!LevelManager.instance.gameOver)
        {
            Time.timeScale = 2;
        }
    }

    public void ToggleSlowDown()
    {
        if (LevelManager.instance.gameOver)
        {
            return;
        }
        if (Time.timeScale == 0.5f)
        {
            Time.timeScale = 1;
            // Hacky way to deselect the button
            slowButton.interactable = false;
            slowButton.interactable = true;
        }
        else
        {
            Time.timeScale = 0.5f;
        }
    }

    public void ToggleSpeedUp()
    {
        if (LevelManager.instance.gameOver)
        {
            return;
        }
        if (Time.timeScale == 2)
        {
            Time.timeScale = 1;
            // Hacky way to deselect the button
            speedButton.interactable = false;
            speedButton.interactable = true;
        }
        else
        {
            Time.timeScale = 2;
        }
    }
}
