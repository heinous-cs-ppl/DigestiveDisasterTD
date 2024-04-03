using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("UI scene");
        Spawner.waveIdx = -1;
        Spawner.waveEnd = true;
        MoneyManager.SetMoneyCount(250);
        PurifyManager.SetMealCount(0);
        Time.timeScale = 1;
    }

    public void Menu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Start Menu");
    }
}
