using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextWave : MonoBehaviour
{
    public static int waveMoney = 100;

    public void ButtonClicked()
    {
        // Call NewWave in Spawner to check if the wave is current wave is done spawning
        Spawner.instance.NewWave();

        UIManager.UpdateRound();
        UIManager.UpdateMove(MoveStudent.moveCost);
    }
}
