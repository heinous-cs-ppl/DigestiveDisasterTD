using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {
    // Wave tracking variables
    private int waves;
    private int waveIdx = 0;   // wave number is (waveIdx+1)
    private Wave.WavePart[] curWaveInfo;

    // timer
    private float t = 0;

    // keeps track of the next enemy index (what enemies to spawn)
    private int i = 0;

    // holds the current enemy data
    private Wave.WavePart curEnemy;
    
    // int to keep track of number of times the spawn has been repeated
    private int repeatCount = 0;

    // array of every spawnpoint
    private Transform[] spawnpoints;
    
    // holds the index of the spawnpoint (since they are indexed in an array)
    private int spawnIndex;

    // holds the spawn point of the next enemy
    private Transform spawnPoint;

    // private bool paths = false;     // This will be set to true in start if LevelManager was given paths to work with


    /* Returns an array of struct defined in Wave.cs */
    private Wave.WavePart[] getWaveInfo(GameObject waveObj) {
        return waveObj.GetComponent<Wave>().waveEnc;  
    }

    /* This function gets called by the user clicking a button, via NextWave.cs */
    public void NewWave() {        
        // Check if the current wave is done spawning, and if there are more waves
        if (waveIdx < (waves-1) && i >= curWaveInfo.Length) {
            // Spawn random vacuous students
            LevelManager.instance.SpawnVacuousStudents();

            // Get new wave information
            waveIdx++;
            curWaveInfo = getWaveInfo(LevelManager.instance.waves[waveIdx]);

            // Set details of first enemy in new wave
            i = 0;
            curEnemy = curWaveInfo[i];
            spawnIndex = curEnemy.enemy.GetComponent<EnemyInfo>().spawnPointIndex;
            spawnPoint = spawnpoints[spawnIndex];
        }
    }

    void Awake() {
        waves = LevelManager.instance.waves.Length;

        if (waves < 1) {
            Debug.Log("NO WAVES TO SPAWN");
            Destroy(gameObject);
        }
        curWaveInfo = getWaveInfo(LevelManager.instance.waves[waveIdx]);
    }

    void Start() {
        // If this runs LevelManager has stuff to work with. This flag is to avoid vacuous console errors
        if (LevelManager.instance.spawnObjs.Length > 0) {
            // Spawn Vacuous students randomly
            LevelManager.instance.SpawnVacuousStudents();

            // get the first enemy in the list
            curEnemy = curWaveInfo[i];
            
            // get the index of the spawnpoint for the first enemy
            spawnIndex = curEnemy.enemy.GetComponent<EnemyInfo>().spawnPointIndex;
            
            // get a list of every spawnpoint's position
            spawnpoints = LevelManager.instance.spawnObjTransforms;

            // get the spawnpoint of the first enemy
            spawnPoint = spawnpoints[spawnIndex];
        } 
    }

    void Update() {   
        // Keep doing nothing at end of wave.
        if(i >= curWaveInfo.Length){
            return;
        }

        t += Time.deltaTime;
        if(t >= curEnemy.spawnDelay){    
            Instantiate(curEnemy.enemy, spawnPoint.position, Quaternion.identity);
            repeatCount++;
            // reset the timer for the next spawn
            t = 0;
            if(repeatCount > curEnemy.repeats){
                // if the spawn has been repeated sufficiently, move to the next spawn in the array
                repeatCount = 0;
                t = 0;
                i++;
                if(i < curWaveInfo.Length)
                {
                    // if there are still more enemies in the array
                    curEnemy = curWaveInfo[i];

                    spawnIndex = curEnemy.enemy.GetComponent<EnemyInfo>().spawnPointIndex;
                    spawnPoint = spawnpoints[spawnIndex];
                    
                }
            }
        }
    }
}

[System.Serializable]
public class EnemyData
{   
    // enemy prefab to spawn
    public GameObject enemy;

    // delay time before spawning the enemy
    public float spawnDelay;

    // number of times to repeat the spawn before moving onto the next enemy in the array
    public int repeats;
}