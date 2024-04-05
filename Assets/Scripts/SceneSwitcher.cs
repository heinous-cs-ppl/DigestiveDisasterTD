using UnityEngine;

public class SceneSwitcher : MonoBehaviour
{
    public void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("UI scene");
    }
}
