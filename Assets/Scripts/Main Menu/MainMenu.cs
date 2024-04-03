using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("UI scene");
        Spawner.waveIdx = -1;
        MoneyManager.SetMoneyCount(250);
        PurifyManager.SetMealCount(0);
    }

    public void Menu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Start Menu");
    }
}
