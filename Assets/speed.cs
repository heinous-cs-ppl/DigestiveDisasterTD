using UnityEngine;

public class slowdown : MonoBehaviour
{
    public UnityEngine.UI.Button slowButton;
    public UnityEngine.UI.Button speedButton;

    public KeyCode slowKey = KeyCode.Q;
    public KeyCode speedKey = KeyCode.W;

    private bool slowKeyLast = false;
    private bool speedKeyLast = false;

    public void Start()
    {
        slowButton.onClick.AddListener(ToggleSlowDown);
        speedButton.onClick.AddListener(ToggleSpeedUp);
    }

    public void Update()
    {
        /* if (!Input.GetKey(slowKey) && !Input.GetKey(speedKey)) */
        /* { */
        /*     Time.timeScale = 1; */
        /* } */
        /* else if (Input.GetKey(slowKey)) */
        /* { */
        /*     if (Input.GetKey(speedKey)) */
        /*     { */
        /*         slowKeyLast = true; */
        /*     } */
        /*     else */
        /*     { */
        /*         slowKeyLast = false; */
        /*         speedKeyLast = false; */
        /*         SlowDown(); */
        /*     } */
        /* } */
        /* else if (Input.GetKey(speedKey)) */
        /* { */
        /*     if (Input.GetKey(slowKey)) */
        /*     { */
        /*         speedKeyLast = true; */
        /*     } */
        /*     else */
        /*     { */
        /*         slowKeyLast = false; */
        /*         speedKeyLast = false; */
        /*         SpeedUp(); */
        /*     } */
        /* } */
        /* if (Input.GetKey(slowKey) && Input.GetKey(speedKey)) */
        /* { */
        /*     if (slowKeyLast) */
        /*     { */
        /*         SlowDown(); */
        /*     } */
        /*     if (speedKeyLast) */
        /*     { */
        /*         SpeedUp(); */
        /*     } */
        /* } */


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
                Time.timeScale = 1;
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
                Time.timeScale = 1;
            }
        }
    }

    public void SlowDown()
    {
        Time.timeScale = 0.5f;
    }

    public void SpeedUp()
    {
        Time.timeScale = 2;
    }

    public void ToggleSlowDown()
    {
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

