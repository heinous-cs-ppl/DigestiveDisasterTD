using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextWave : MonoBehaviour
{
    public void ButtonClicked() {
        // Call NewWave() in the script belonging to the game Spawner
        Spawner spawner = LevelManager.instance.spawner.GetComponent<Spawner>();
        spawner.NewWave();
    }
}
