using UnityEngine;

public class speedup : MonoBehaviour
{
    public UnityEngine.UI.Button button;
    public void SpeedUp()
    {
        if (Time.timeScale == 2)
        {
            Time.timeScale = 1;
            // Hacky way to deselect the button
            button.interactable = false;
            button.interactable = true;
        }
        else
        {
            Time.timeScale = 2;
            Debug.Log("Time scale set to 2");
        }
    }
}
