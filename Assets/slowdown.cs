using UnityEngine;

public class slowdown : MonoBehaviour
{
    public UnityEngine.UI.Button button;

    public void SlowDown()
    {
        if (Time.timeScale == 0.5f)
        {
            Time.timeScale = 1;
            // Hacky way to deselect the button
            button.interactable = false;
            button.interactable = true;
        }
        else
        {
            Time.timeScale = 0.5f;
        }
    }
}
