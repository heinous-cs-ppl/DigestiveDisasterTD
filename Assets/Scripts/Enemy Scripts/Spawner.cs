using UnityEngine;
// using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;


public class Spawner : MonoBehaviour
{
    // Wave tracking variables
    private static int waves;
    private static int waveIdx = -1;   // wave number is (waveIdx+1)
    private Wave.WavePart[][] curWaveInfo;
    
    // timer
    // private float[] t = new float[3];    // doesnt work in coroutine for some reason

    private bool[] isNewEnemy = new bool[3];

    // holds the current enemy data
    private Wave.WavePart[] curWaveElem = new Wave.WavePart[3];

    // keeps track of the next enemy index (what enemies to spawn)
    private int[] i = new int[3];

    // int to keep track of number of times the spawn has been repeated
    private int[] repeatCount = new int[3];

    // array of every spawnpoint
    private Transform[] spawnpoints;

    // holds the index of the spawnpoint (since they are indexed in an array)
    private int[] spawnIndex = new int[3];

    // // holds the spawn point of the next enemy
    // private Transform spawnPoint;

    private bool[] spawn = {false, false, false};
    private bool anySpawning = false;
    private bool enemiesAlive = false;
    public static bool waveEnd = true;

    public Button nextWaveButton;

    public static bool roundAddMoney = false;

    /* Returns an array of struct defined in Wave.cs */
    private Wave.WavePart[][] getWaveInfo(GameObject waveObj)
    {
        // 3 arrays of WavePart[] corresponding to each spawner
        Wave.WavePart[][] allSpawnsWaveInfo = new Wave.WavePart[3][];
        allSpawnsWaveInfo[0] = waveObj.GetComponent<Wave>().Spawner0WaveEnc;
        allSpawnsWaveInfo[1] = waveObj.GetComponent<Wave>().Spawner1WaveEnc;
        allSpawnsWaveInfo[2] = waveObj.GetComponent<Wave>().Spawner2WaveEnc;
        return allSpawnsWaveInfo;
    }

    /* This function gets called by the user clicking a button, via NextWave.cs */
    public void NewWave()
    {
        // If wave is done and more waves exist, start wave
        if (waveEnd && waveIdx < (waves - 1)) {
            // Spawn random vacuous students
            // LevelManager.instance.SpawnVacuousStudents();
            nextWaveButton.interactable = false;
            roundAddMoney = true;



            // Get new wave information
            waveIdx++;
            curWaveInfo = getWaveInfo(LevelManager.instance.waves[waveIdx]);
            i[0] = 0;
            i[1] = 0;
            i[2] = 0;
            isNewEnemy[0] = true;
            isNewEnemy[1] = true;
            isNewEnemy[2] = true;
            repeatCount[0] = 0;
            repeatCount[1] = 0;
            repeatCount[2] = 0;
            spawn[0] = true;
            spawn[1] = true;
            spawn[2] = true;
            // curEnemy[0] = curWaveSpawner0Info[i[0]];
            // These will all run simultaneously
            StartCoroutine(SpawnerThread0());
            StartCoroutine(SpawnerThread1());
            StartCoroutine(SpawnerThread2());

            anySpawning = true;
            waveEnd = false;
        }
        Debug.Log("Wave "+waveIdx);
    }

    void Awake()
    {
        waves = LevelManager.instance.waves.Length;

        if (waves < 1)
        {
            Debug.Log("NO WAVES TO SPAWN");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // If this runs LevelManager has spawnpoints to work with. This flag is to avoid vacuous console errors
        if (LevelManager.instance.spawnObjs.Length > 0)
        {
            // Spawn Vacuous students randomly
            LevelManager.instance.SpawnVacuousStudents();

            // get a list of every spawnpoint's position
            spawnpoints = LevelManager.instance.spawnObjTransforms;
        }
    }

    private IEnumerator SpawnerThread0() {
        while (spawn[0]) {
            if (curWaveInfo[0].Length > 0)
            {
                if (isNewEnemy[0]) {      // First time seeing this element (enemy)
                    curWaveElem[0] = curWaveInfo[0][i[0]];
                    spawnIndex[0] = curWaveElem[0].enemy.GetComponent<EnemyInfo>().spawnPointIndex;
                    yield return new WaitForSeconds(curWaveElem[0].oneTimeDelay);
                    isNewEnemy[0] = false;
                }

                // Spawn enemy and repeat relevent times
                if (repeatCount[0] <= curWaveElem[0].repeats) {
                    Instantiate(curWaveElem[0].enemy, spawnpoints[spawnIndex[0]].position, Quaternion.identity);
                    repeatCount[0]++;
                    yield return new WaitForSeconds(curWaveElem[0].spawnDelay);
                }
                else    // Move onto the next element
                {
                    i[0]++;
                    isNewEnemy[0] = true;
                    repeatCount[0] = 0;
                }
            }

            if (i[0] >= curWaveInfo[0].Length || curWaveInfo[0].Length == 0) {
                spawn[0] = false;
                Debug.Log("Spawner0 done");
            }
        }
    }
    private IEnumerator SpawnerThread1() {
        while (spawn[1]) {
            if (curWaveInfo[1].Length > 0)
            {
                if (isNewEnemy[1]) {      // First time seeing this element (enemy)
                    curWaveElem[1] = curWaveInfo[1][i[1]];
                    spawnIndex[1] = curWaveElem[1].enemy.GetComponent<EnemyInfo>().spawnPointIndex;
                    yield return new WaitForSeconds(curWaveElem[1].oneTimeDelay);
                    isNewEnemy[1] = false;
                }

                // Spawn enemy and repeat relevent times
                if (repeatCount[1] <= curWaveElem[1].repeats) {
                    Instantiate(curWaveElem[1].enemy, spawnpoints[spawnIndex[1]].position, Quaternion.identity);
                    repeatCount[1]++;
                    yield return new WaitForSeconds(curWaveElem[1].spawnDelay);
                }
                else    // Move onto the next element
                {
                    i[1]++;
                    isNewEnemy[1] = true;
                    repeatCount[1] = 0;
                }
            }

            if (i[1] >= curWaveInfo[1].Length || curWaveInfo[1].Length == 0) {
                spawn[1] = false;
                Debug.Log("Spawner1 done");
            }
        }
    }
    private IEnumerator SpawnerThread2() {
        while (spawn[2]) {
            if (curWaveInfo[2].Length > 0)
            {
                if (isNewEnemy[2]) {      // First time seeing this element (enemy)
                    curWaveElem[2] = curWaveInfo[2][i[2]];
                    spawnIndex[2] = curWaveElem[2].enemy.GetComponent<EnemyInfo>().spawnPointIndex;
                    yield return new WaitForSeconds(curWaveElem[2].oneTimeDelay);
                    isNewEnemy[2] = false;
                }

                // Spawn enemy and repeat relevent times
                if (repeatCount[2] <= curWaveElem[2].repeats) {
                    Instantiate(curWaveElem[2].enemy, spawnpoints[spawnIndex[2]].position, Quaternion.identity);
                    repeatCount[2]++;
                    yield return new WaitForSeconds(curWaveElem[2].spawnDelay);
                }
                else    // Move onto the next element
                {
                    i[2]++;
                    isNewEnemy[2] = true;
                    repeatCount[2] = 0;
                }
            }

            if (i[2] >= curWaveInfo[2].Length || curWaveInfo[2].Length == 0) {
                spawn[2] = false;
                Debug.Log("Spawner2 done");
            }
        }
    }

    void Update()
    {
        anySpawning = spawn[0] || spawn[1] || spawn[2];
        if (!anySpawning && waveIdx < (waves - 1) && roundAddMoney) {
            // nextWaveButton.interactable = true;
            // MoneyManager.AddMoney(NextWave.waveMoney);
            // UIManager.UpdateMoney();
            // roundAddMoney = false;
        }

        RaycastHit2D[] hitEnemies = Physics2D.CircleCastAll(Vector2.zero, Mathf.Infinity, Vector2.zero, Mathf.Infinity, LevelManager.instance.enemyLayer);
        if (hitEnemies.Length == 0 && enemiesAlive) {
            enemiesAlive = false;
            Debug.Log("no enemies alive");
        } else if (hitEnemies.Length > 0 && !enemiesAlive) {
            enemiesAlive = true;
            Debug.Log("enemies alive");
        }

        if (!(anySpawning || enemiesAlive) && !waveEnd) {
            waveEnd = true;
            nextWaveButton.interactable = true;
            Debug.Log("wave ended");
            UIManager.UpdateMove(0);
            LevelManager.instance.SpawnVacuousStudents();
            MoneyManager.AddMoney(NextWave.waveMoney);
            UIManager.UpdateMoney();
        }
    }

    public static int GetRound() {
        return waveIdx;
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