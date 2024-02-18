using UnityEngine;
using System.Collections.Generic;



public class Spawner : MonoBehaviour
{
    public EnemyData[] enemies;

    // keeps track of the next enemy index
    private int i = 0;

    // timer
    private float t = 0;

    // holds the current enemy data
    private EnemyData cur;
    
    // holds the index of the spawnpoint (since they are indexed in an array)
    private int spawnIndex;

    // holds the spawn point of the next enemy
    private Vector2 nextSpawnPoint;
    
    // another class to get the spawnpoints
    private FindSpawnpoint spawnpointFind;

    // array of every spawnpoint
    private Vector2[] spawnpoints;

    // int to keep track of number of times the spawn has been repeated
    private int repeatCount = 0;

    void Start()
    {
        // get the first enemy in the list
        cur = enemies[i];
        
        // get the index of the spawnpoint for the first enemy
        spawnIndex = cur.enemy.GetComponent<EnemyInfo>().spawnPointIndex;
        
        // get a list of every spawnpoint's position
        spawnpointFind = new FindSpawnpoint();
        // GetSpawnpoints returns an array of Vector2 that hold the positions of every spawnpoint
        spawnpoints = spawnpointFind.GetSpawnpointPositions();
        // get the spawnpoint of the first enemy
        nextSpawnPoint = spawnpoints[spawnIndex];

    }

    void Update()
    {   
        if(i >= enemies.Length){
            return;
        }
        t += Time.deltaTime;
        if(t >= cur.spawnDelay){
            
            Instantiate(cur.enemy, nextSpawnPoint, Quaternion.identity);
            repeatCount++;
            // reset the timer for the next spawn
            t = 0;
            if(repeatCount > cur.repeats){
                // if the spawn has been repeated sufficiently, move to the next spawn in the array
                repeatCount = 0;
                t = 0;
                i++;
                if(i < enemies.Length)
                {
                    // if there are still more enemies in the array
                    cur = enemies[i];

                    spawnIndex = cur.enemy.GetComponent<EnemyInfo>().spawnPointIndex;
                    nextSpawnPoint = spawnpoints[spawnIndex];
                    
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