using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    /********************* References *********************/
    public static LevelManager instance; // Use this reference to access stuff from this class

    [Header("References")]
    public GameObject spawner;

    [SerializeField]
    private GameObject plotsParent; // The parent object of all plots
    public LayerMask plotLayer;
    public LayerMask enemyLayer;

    [SerializeField]
    private GameObject vacuousStudent;
    public GameObject machineRepresentation;

    // Number of starting paths or number of spawnpoints - each of these points is the parent of the rest of it's path
    public GameObject[] spawnObjs; // given in object inspector

    // Array of wave game objects
    public GameObject[] waves; // Wave prefabs given in Unity Inspector - these contain info about enemies

    // Transform version of spawnObjs
    [HideInInspector]
    public Transform[] spawnObjTransforms;

    private int studentCount = 1;

    [HideInInspector]
    public int studentsDead = 0;
    public int deathLimit = 10;
    [SerializeField] private const int VACUOUS_STUDENT_LIMIT = 15;
    public int vacuousAlive = 0;

    public bool disableGameOver = false;
    public bool gameOver = false;

    // used to disable mouse interaction with AOE
    [SerializeField]
    private LayerMask inputLayerMask;

    // Like Start() but is called first, after all objects and therefore scripts are initialized
    private void Awake()
    {
        instance = this;

        if (spawnObjs.Length < 1)
        {
            Debug.LogError("LevelManager: NO PATHS GIVEN");
            Debug.Break();
        }

        spawnObjTransforms = new Transform[spawnObjs.Length];
        for (int i = 0; i < spawnObjs.Length; i++)
        {
            spawnObjTransforms[i] = spawnObjs[i].transform;
        }
        Camera.main.eventMask = inputLayerMask;
    }

    /* Since the paths stop at a point, then that point has child objects to represent multiple paths, each of which is the parent of
    the rest of the points that make up that path */
    public Transform[] NextPathLeg(GameObject endNode, bool spawnPoint)
    {
        int childCount = endNode.transform.childCount; // Number of paths the enemy could take from this endNode
        if (childCount == 0)
        {
            return null;
        }

        Transform[] nextPathLeg;
        if (spawnPoint)
        {
            // Return a list of the full path under this spawnpoint
            nextPathLeg = new Transform[childCount];
            for (int i = 0; i < childCount; i++)
            {
                nextPathLeg[i] = endNode.transform.GetChild(i);
            }
        }
        else
        {
            // Choose which path to take randomly
            int nextPathIndex = Random.Range(0, childCount);
            Transform nextNode = endNode.transform.GetChild(nextPathIndex);

            // Return a list of the full path under this randomly selected path to follow, where nextNode is the first entry of the list
            nextPathLeg = new Transform[nextNode.childCount + 1];
            for (int i = 0; i < nextNode.childCount + 1; i++)
            {
                if (i == 0)
                {
                    nextPathLeg[i] = nextNode;
                }
                else
                {
                    // These Transform objs are made from GameObjects, so the GameObject obj can be accessed with the .gameObject property
                    nextPathLeg[i] = nextNode.transform.GetChild(i - 1);
                }
            }
        }
        return nextPathLeg;
    }

    /* Calculates an array of all plots that do not have a student on them, then randomly selects a few of these plots to place vacuous students
        on, assigning them to be occupied by a vacuous student so that nothing can be placed on these plots before the vacuous students make it on. */
    public Plot[] AssignVacuousStudents()
    {
        int plotCount = plotsParent.transform.childCount;
        Plot[] plots = new Plot[plotCount];

        // Get list of all plots
        for (int i = 0; i < plotCount; i++)
        {
            plots[i] = plotsParent.transform.GetChild(i).gameObject.GetComponent<Plot>();
        }

        // Find out how many free plots exist
        int free = 0;
        foreach (Plot plot in plots)
        {
            if (plot.student == null)
            {
                free++;
            }
            else
            {
                Debug.Log(plot.name + " is occupied by " + plot.student.name);
            }
        }

        if (free > 0)
        {
            // Now initialize and fill an array with these free plots
            Plot[] freeplots = new Plot[free];
            for (int i = 0, p = 0; i < plotCount; i++)
            {
                if (plots[i].student == null)
                {
                    freeplots[p] = plots[i];
                    p++;
                }
            }

            // Randomly select a how many plots plots will get vacuous students
            // int numVac = Random.Range(2, Mathf.Min(6, free + 1));    // 2 to 5 vacuous students can come at most
            int allowableVac = VACUOUS_STUDENT_LIMIT - vacuousAlive;
            int numVac = Mathf.Min(Mathf.Min(3, free), allowableVac); // spawns 3 students, unless there are less than 3 free plots or too many vacuous students
            int[] vacPlotIdxs = new int[numVac];
            Plot[] vacPlots = new Plot[numVac];

            // create a list containing numbers 1 to free
            List<int> freeIdxs = new List<int>();
            for (int i = 0; i < free; i++)
            {
                freeIdxs.Add(i);
            }

            // select numVac random numbers from the array of free indices
            for (int i = 0; i < numVac; i++)
            {
                int randIdx = Random.Range(0, free - i);
                vacPlotIdxs[i] = freeIdxs[randIdx];
                // remove the selected index from the array
                freeIdxs.RemoveAt(randIdx);
            }

            // Randomly select the indices of the freeplots to place vacuous students on
            // for (int i = 0; i < numVac; i++)
            // {
            //     bool dupes = true;
            //     while (dupes)
            //     {
            //         vacPlotIdxs[i] = Random.Range(0, free);
            //         dupes = false;

            //         // Check if a duplicate index was generated
            //         for (int j = 0; j < i; j++)
            //         {
            //             if (vacPlotIdxs[j] == vacPlotIdxs[i])
            //             {
            //                 dupes = true;
            //             }
            //         }
            //     }
            // }

            // Get the plot objects and set to be occupied
            for (int i = 0; i < numVac; i++)
            {
                vacPlots[i] = freeplots[vacPlotIdxs[i]];
                vacPlots[i].student = vacuousStudent;
            }
            return vacPlots;
        }
        return null;
    }

    public void SpawnVacuousStudents()
    {
        if (vacuousAlive < VACUOUS_STUDENT_LIMIT)
        {
            // Assign vacuous students to plots
            Plot[] vac = AssignVacuousStudents();

            // Spawn the vacuous students on the plots if there is space
            if (vac != null)
            {
                foreach (Plot plot in vac)
                {
                    Vector2 studentPosition = plot.transform.position;
                    GameObject placed = Instantiate(
                        vacuousStudent,
                        studentPosition,
                        Quaternion.identity
                    );
                    vacuousAlive++;

                    // Giving name of student in order for healing turret bullet collision to be able to differentiate between the different students
                    placed.name = "Vacuous" + studentCount;
                    studentCount++;
                    if (plot.GetComponent<Plot>().aboveTable)
                        placed.GetComponent<SpriteRenderer>().sortingLayerName =
                            "Students above tables";
                }
            }
        }
    }

    public void GameOver()
    {
        if (!disableGameOver)
        {
            Time.timeScale = 0;
            gameOver = true;
            UIManager.instance.GameOver();
        }
    }
}
