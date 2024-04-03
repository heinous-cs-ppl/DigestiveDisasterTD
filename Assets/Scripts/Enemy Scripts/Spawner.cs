// using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;

    // Wave tracking variables
    private int waves;
    private int waveIdx = -1; // wave number is (waveIdx+1)
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

    private bool[] spawn = { false, false, false };
    public bool anySpawning = false;
    private bool enemiesAlive = false;
    public bool waveEnd = true;

    public Button nextWaveButton;

    public bool roundAddMoney = false;

    // boss stuff
    // false if the current wave is not a boss wave, true if it is
    public bool isBossWave = false;

    // false if there is no boss, true if the boss is alive
    public bool bossAlive = false;

    // holds tags for each enemy spawning in the boss waves
    private string[] bossSpawnerThreads = new string[3];

    // holds the number of each enemy alive in the boss wave
    private int[] bossEnemyCount = new int[3];

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
        if (waveEnd && waveIdx < (waves - 1))
        {
            // Spawn random vacuous students
            // LevelManager.instance.SpawnVacuousStudents();
            nextWaveButton.interactable = false;
            roundAddMoney = true;

            UIManager.HidePath();

            // Get new wave information
            waveIdx++;
            curWaveInfo = getWaveInfo(LevelManager.instance.waves[waveIdx]);

            waveEnd = false;
            anySpawning = true;
            // if the wave is not a boss wave
            if (!LevelManager.instance.waves[waveIdx].GetComponent<Wave>().bossWave)
            {
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
            }
            else
            {
                // if the wave is a boss wave
                // The boss wave will have up to three spawners, with one enemy each
                // The "repeats" field in the struct will be treated as "maxInstances"
                // meaning that there will be maxInstances of the enemy alive while the boss is alive
                // enemies will stop spawning once boss is dead (anySpawning set to false once boss dies)

                // set boss to alive
                bossAlive = true;
                isBossWave = true;

                // spawn the boss
                StartCoroutine(
                    SpawnBoss(LevelManager.instance.waves[waveIdx].GetComponent<BossWave>().boss)
                );

                // set the enemies in spawner threads to keep track of the number of each enemy alive
                Wave.WavePart enemy0;
                Wave.WavePart enemy1;
                Wave.WavePart enemy2;
                if (curWaveInfo[0].Length > 0)
                {
                    enemy0 = curWaveInfo[0][0];
                    bossSpawnerThreads[0] = enemy0.enemy.tag;
                    StartCoroutine(BossWaveSpawner(enemy0));
                }
                if (curWaveInfo[1].Length > 0)
                {
                    enemy1 = curWaveInfo[1][0];
                    bossSpawnerThreads[1] = enemy1.enemy.tag;
                    StartCoroutine(BossWaveSpawner(enemy1));
                }
                if (curWaveInfo[2].Length > 0)
                {
                    enemy2 = curWaveInfo[2][0];
                    bossSpawnerThreads[2] = enemy2.enemy.tag;
                    StartCoroutine(BossWaveSpawner(enemy2));
                }
            }
        }
        Debug.Log("Wave " + waveIdx);
    }

    public IEnumerator SpawnBoss(Wave.WavePart boss)
    {
        yield return new WaitForSeconds(boss.oneTimeDelay);
        // spawn the boss
        GameObject chymousDisaster = Instantiate(
            boss.enemy,
            spawnpoints[boss.enemy.GetComponent<EnemyInfo>().spawnPointIndex].position,
            Quaternion.identity
        );
        EnemyInfo chymousInfo = chymousDisaster.GetComponent<EnemyInfo>();
        chymousInfo.isBoss = true;
        chymousDisaster.GetComponent<EnemyAttacks>().isBoss = true;

        // multiply hp and purify hp by the value set in repeats (chymous and heinous code)
        chymousInfo.maxHp *= boss.repeats;
        chymousInfo.healthBar.setMaxValue(chymousInfo.maxHp);
        chymousInfo.healthBar.setValue(chymousInfo.maxHp);

        chymousInfo.maxPurifyHp *= boss.repeats;
        chymousInfo.purifyBar.setMaxValue(chymousInfo.maxPurifyHp);
        // don't need to set purify bar value since it starts at 0

        // halve the speed of the boss
        chymousInfo.speed /= 2f;

        // double the scale of the boss
        chymousDisaster.transform.localScale *= 2f;
    }

    public int GetBossWaveSpawnerThread(string tag)
    {
        // return the spawner thread of the corresponding tag
        for (int i = 0; i < bossSpawnerThreads.Length; i++)
        {
            if (bossSpawnerThreads[i].Equals(tag))
            {
                return i;
            }
        }
        throw new System.Exception("No spawner thread found for tag " + tag);
    }

    public void ReduceBossEnemyCount(string tag)
    {
        // reduce the enemy count of the corresponding tag
        int spawnerThread = GetBossWaveSpawnerThread(tag);
        bossEnemyCount[spawnerThread]--;
        Debug.Log(tag + " removed, count: " + bossEnemyCount[spawnerThread]);
    }

    private IEnumerator BossWaveSpawner(Wave.WavePart chyme)
    {
        yield return new WaitForSeconds(chyme.oneTimeDelay);
        int spawnerThread = GetBossWaveSpawnerThread(chyme.enemy.tag);
        while (bossAlive)
        {
            // check if the enemy limit has been reached
            // wait to spawn the enemy
            if (bossEnemyCount[spawnerThread] < chyme.repeats)
            {
                // spawn the enemy
                Instantiate(
                    chyme.enemy,
                    spawnpoints[chyme.enemy.GetComponent<EnemyInfo>().spawnPointIndex].position,
                    Quaternion.identity
                );
                bossEnemyCount[spawnerThread]++;
                Debug.Log(chyme.enemy.tag + " spawned, count: " + bossEnemyCount[spawnerThread]);
                yield return new WaitForSeconds(chyme.spawnDelay);
            }
            else
            {
                // wait for one frame (game freeze if you don't)
                yield return null;
            }
        }
    }

    void Awake()
    {
        instance = this;
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
            UIManager.UpdateMove(0);
            ShowPath();
        }
    }

    private IEnumerator SpawnerThread0()
    {
        while (spawn[0])
        {
            if (curWaveInfo[0].Length > 0)
            {
                if (isNewEnemy[0])
                { // First time seeing this element (enemy)
                    curWaveElem[0] = curWaveInfo[0][i[0]];
                    spawnIndex[0] = curWaveElem[0].enemy.GetComponent<EnemyInfo>().spawnPointIndex;
                    yield return new WaitForSeconds(curWaveElem[0].oneTimeDelay);
                    isNewEnemy[0] = false;
                }

                // Spawn enemy and repeat relevent times
                if (repeatCount[0] <= curWaveElem[0].repeats)
                {
                    Instantiate(
                        curWaveElem[0].enemy,
                        spawnpoints[spawnIndex[0]].position,
                        Quaternion.identity
                    );
                    repeatCount[0]++;
                    yield return new WaitForSeconds(curWaveElem[0].spawnDelay);
                }
                else // Move onto the next element
                {
                    i[0]++;
                    isNewEnemy[0] = true;
                    repeatCount[0] = 0;
                }
            }

            if (i[0] >= curWaveInfo[0].Length || curWaveInfo[0].Length == 0)
            {
                spawn[0] = false;
                Debug.Log("Spawner0 done");
            }
        }
    }

    private IEnumerator SpawnerThread1()
    {
        while (spawn[1])
        {
            if (curWaveInfo[1].Length > 0)
            {
                if (isNewEnemy[1])
                { // First time seeing this element (enemy)
                    curWaveElem[1] = curWaveInfo[1][i[1]];
                    spawnIndex[1] = curWaveElem[1].enemy.GetComponent<EnemyInfo>().spawnPointIndex;
                    yield return new WaitForSeconds(curWaveElem[1].oneTimeDelay);
                    isNewEnemy[1] = false;
                }

                // Spawn enemy and repeat relevent times
                if (repeatCount[1] <= curWaveElem[1].repeats)
                {
                    Instantiate(
                        curWaveElem[1].enemy,
                        spawnpoints[spawnIndex[1]].position,
                        Quaternion.identity
                    );
                    repeatCount[1]++;
                    yield return new WaitForSeconds(curWaveElem[1].spawnDelay);
                }
                else // Move onto the next element
                {
                    i[1]++;
                    isNewEnemy[1] = true;
                    repeatCount[1] = 0;
                }
            }

            if (i[1] >= curWaveInfo[1].Length || curWaveInfo[1].Length == 0)
            {
                spawn[1] = false;
                Debug.Log("Spawner1 done");
            }
        }
    }

    private IEnumerator SpawnerThread2()
    {
        while (spawn[2])
        {
            if (curWaveInfo[2].Length > 0)
            {
                if (isNewEnemy[2])
                { // First time seeing this element (enemy)
                    curWaveElem[2] = curWaveInfo[2][i[2]];
                    spawnIndex[2] = curWaveElem[2].enemy.GetComponent<EnemyInfo>().spawnPointIndex;
                    yield return new WaitForSeconds(curWaveElem[2].oneTimeDelay);
                    isNewEnemy[2] = false;
                }

                // Spawn enemy and repeat relevent times
                if (repeatCount[2] <= curWaveElem[2].repeats)
                {
                    Instantiate(
                        curWaveElem[2].enemy,
                        spawnpoints[spawnIndex[2]].position,
                        Quaternion.identity
                    );
                    repeatCount[2]++;
                    yield return new WaitForSeconds(curWaveElem[2].spawnDelay);
                }
                else // Move onto the next element
                {
                    i[2]++;
                    isNewEnemy[2] = true;
                    repeatCount[2] = 0;
                }
            }

            if (i[2] >= curWaveInfo[2].Length || curWaveInfo[2].Length == 0)
            {
                spawn[2] = false;
                Debug.Log("Spawner2 done");
            }
        }
    }

    void Update()
    {
        anySpawning = spawn[0] || spawn[1] || spawn[2];
        // if (!anySpawning && waveIdx < (waves - 1) && roundAddMoney) {
        //     nextWaveButton.interactable = true;
        //     MoneyManager.AddMoney(NextWave.waveMoney);
        //     UIManager.UpdateMoney();
        //     roundAddMoney = false;
        // }

        RaycastHit2D[] hitEnemies = Physics2D.CircleCastAll(
            Vector2.zero,
            Mathf.Infinity,
            Vector2.zero,
            Mathf.Infinity,
            LevelManager.instance.enemyLayer
        );
        if (hitEnemies.Length == 0 && enemiesAlive)
        {
            enemiesAlive = false;
            Debug.Log("no enemies alive");
        }
        else if (hitEnemies.Length > 0 && !enemiesAlive)
        {
            enemiesAlive = true;
            Debug.Log("enemies alive");
        }

        if (!(anySpawning || enemiesAlive) && !waveEnd && !isBossWave)
        {
            // when a regular wave ends
            waveEnd = true;
            nextWaveButton.interactable = true;
            Debug.Log("wave ended");
            UIManager.UpdateMove(0);
            LevelManager.instance.SpawnVacuousStudents();
            MoneyManager.AddMoney(NextWave.waveMoney);
            UIManager.UpdateMoney();
            ShowPath();
        }
        else if (isBossWave && !bossAlive && !enemiesAlive && !waveEnd)
        {
            // when a boss wave ends
            waveEnd = true;
            nextWaveButton.interactable = true;
            Debug.Log("boss wave ended");
            UIManager.UpdateMove(0);
            LevelManager.instance.SpawnVacuousStudents();
            MoneyManager.AddMoney(NextWave.waveMoney);
            UIManager.UpdateMoney();
            ShowPath();

            // reset boss related fields
            isBossWave = false;
            bossSpawnerThreads = new string[3];
            bossEnemyCount = new int[3];
        }
    }

    public int GetRound()
    {
        if (waveIdx == -1)
        {
            return 1;
        }
        return waveIdx + 1;
    }

    void ShowPath()
    {
        // get next wave
        GameObject wave = LevelManager.instance.waves[waveIdx + 1];
        Wave.WavePart[] spawner0 = wave.GetComponent<Wave>().Spawner0WaveEnc;
        Wave.WavePart[] spawner1 = wave.GetComponent<Wave>().Spawner1WaveEnc;
        Wave.WavePart[] spawner2 = wave.GetComponent<Wave>().Spawner2WaveEnc;

        bool spawnpoint0 = false;
        bool spawnpoint1 = false;
        bool spawnpoint2 = false;
        // check if there are enemies that spawn at each spawnpoint
        foreach (Wave.WavePart wavePart in spawner0)
        {
            EnemyInfo curEnemy = wavePart.enemy.GetComponent<EnemyInfo>();
            if (curEnemy.spawnPointIndex == 0)
            {
                Debug.Log("spawnpoint0 in spawner0");
                spawnpoint0 = true;
            }
            else if (curEnemy.spawnPointIndex == 1)
            {
                Debug.Log("spawnpoint1 in spawner0");
                spawnpoint1 = true;
            }
            else if (curEnemy.spawnPointIndex == 2)
            {
                Debug.Log("spawnpoint2 in spawner0");
                spawnpoint2 = true;
            }
        }

        foreach (Wave.WavePart wavePart in spawner1)
        {
            EnemyInfo curEnemy = wavePart.enemy.GetComponent<EnemyInfo>();
            if (curEnemy.spawnPointIndex == 0)
            {
                Debug.Log("spawnpoint0 in spawner1");
                spawnpoint0 = true;
            }
            else if (curEnemy.spawnPointIndex == 1)
            {
                Debug.Log("spawnpoint1 in spawner1");
                spawnpoint1 = true;
            }
            else if (curEnemy.spawnPointIndex == 2)
            {
                Debug.Log("spawnpoint2 in spawner1");
                spawnpoint2 = true;
            }
        }

        foreach (Wave.WavePart wavePart in spawner2)
        {
            EnemyInfo curEnemy = wavePart.enemy.GetComponent<EnemyInfo>();
            if (curEnemy.spawnPointIndex == 0)
            {
                Debug.Log("spawnpoint0 in spawner2");
                spawnpoint0 = true;
            }
            else if (curEnemy.spawnPointIndex == 1)
            {
                Debug.Log("spawnpoint1 in spawner2");
                spawnpoint1 = true;
            }
            else if (curEnemy.spawnPointIndex == 2)
            {
                Debug.Log("spawnpoint2 in spawner2");
                spawnpoint2 = true;
            }
        }

        // show the path of the next wave depending on where the enemies are spawning
        UIManager.ShowPath(spawnpoint0, spawnpoint1, spawnpoint2);
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
