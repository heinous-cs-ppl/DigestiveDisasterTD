using UnityEngine;
using UnityEngine.UI;

public class SpeedManager : MonoBehaviour
{
    public static SpeedManager instance;
    public UnityEngine.UI.Button slowButton;
    public UnityEngine.UI.Button speedButton;

    public KeyCode slowKey = KeyCode.Q;
    public KeyCode speedKey = KeyCode.E;

    public Sprite fastOn;
    public Sprite fastOff;
    public Sprite slowOn;
    public Sprite slowOff;
    public Image fastImage;
    public Image slowImage;

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
        if (PauseManager.instance.paused)
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
            fastImage.sprite = fastOff;
            slowImage.sprite = slowOff;
            Time.timeScale = 1;
        }
    }

    public void SlowDown()
    {
        if (!LevelManager.instance.gameOver)
        {
            fastImage.sprite = fastOff;
            slowImage.sprite = slowOn;
            Time.timeScale = 0.5f;
        }
    }

    public void SpeedUp()
    {
        if (!LevelManager.instance.gameOver)
        {
            fastImage.sprite = fastOn;
            slowImage.sprite = slowOff;
            Time.timeScale = 2;
        }
    }

    public void ToggleSlowDown()
    {
        fastImage.sprite = fastOff;
        if (LevelManager.instance.gameOver)
        {
            return;
        }
        if (Time.timeScale == 0.5f)
        {
            Time.timeScale = 1;
            slowImage.sprite = slowOff;
        }
        else
        {
            Time.timeScale = 0.5f;
            slowImage.sprite = slowOn;
        }
    }

    public void ToggleSpeedUp()
    {
        slowImage.sprite = slowOff;
        if (LevelManager.instance.gameOver)
        {
            return;
        }
        if (Time.timeScale == 2)
        {
            Time.timeScale = 1;
            fastImage.sprite = fastOff;
        }
        else
        {
            Time.timeScale = 2;
            fastImage.sprite = fastOn;
        }
    }
}
