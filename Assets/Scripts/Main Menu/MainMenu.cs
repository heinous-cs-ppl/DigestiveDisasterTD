using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("UI scene");
    }
}
