using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    /********************* References *********************/
    public static LevelManager instance;                // Use this reference to access stuff from this class
    
    [Header("References")]
    public GameObject spawner;
    [SerializeField] private GameObject plotsParent;    // The parent object of all plots
    public LayerMask plotLayer;
    [SerializeField] private GameObject vacuousStudent;

    // Number of starting paths or number of spawnpoints - each of these points is the parent of the rest of it's path
    public GameObject[] spawnObjs;      // given in object inspector

    // Array of wave game objects
    public GameObject[] waves;          // Wave prefabs given in Unity Inspector - these contain info about enemies

    // Transform version of spawnObjs
    [HideInInspector] public Transform[] spawnObjTransforms;      


    // Like Start() but is called first, after all objects and therefore scripts are initialized
    private void Awake() {
        instance = this;

        if (spawnObjs.Length < 1) {
            Debug.LogError("LevelManager: NO PATHS GIVEN");
            Debug.Break();
        }

        spawnObjTransforms = new Transform[spawnObjs.Length];   // GameObject arrays use .Length
        for (int i = 0; i < spawnObjs.Length; i++) {
            spawnObjTransforms[i] = spawnObjs[i].transform;
        }
    }

    /* Since the paths stop at a point, then that point has child objects to represent multiple paths, each of which is the parent of
    the rest of the points that make up that path */
    public Transform[] NextPathLeg(GameObject endNode, bool spawnPoint) {
        int childCount = endNode.transform.childCount;      // Number of paths the enemy could take from this endNode
        if (childCount == 0) {
            return null;
        }

        Transform[] nextPathLeg;
        if (spawnPoint) {
            // Return a list of the full path under this spawnpoint
            nextPathLeg = new Transform[childCount];
            for (int i = 0; i < childCount; i++) {
                nextPathLeg[i] = endNode.transform.GetChild(i);
            }
        }
        else {
            // Choose which path to take randomly
            int nextPathIndex = Random.Range(0, childCount);
            Transform nextNode = endNode.transform.GetChild(nextPathIndex);

            // Return a list of the full path under this randomly selected path to follow, where nextNode is the first entry of the list
            nextPathLeg = new Transform[nextNode.childCount + 1];
            for (int i = 0; i < nextNode.childCount + 1; i++) {
                if (i == 0) {
                    nextPathLeg[i] = nextNode;
                }
                else {
                    // These Transform objs are made from GameObjects, so the GameObject obj can be accessed with the .gameObject property
                    nextPathLeg[i] = nextNode.transform.GetChild(i-1);
                }
            }
        }
        return nextPathLeg;
    }

    /* Calculates an array of all plots that do not have a student on them, then randomly selects a few of these plots to place vacuous students
        on, assigning them to be occupied by a vacuous student so that nothing can be placed on these plots before the vacuous students make it on. */
    public Plot[] AssignVacuousStudents() {
        int plotCount = plotsParent.transform.childCount;
        Plot[] plots = new Plot[plotCount];

        // Get list of all plots
        for (int i = 0; i < plotCount; i++) {
            plots[i] = plotsParent.transform.GetChild(i).gameObject.GetComponent<Plot>();
        }


        // Find out how many free plots exist
        int free = 0;
        foreach (Plot plot in plots) {
            if (plot.student == null) {
                free++;
            }
        }

        if (free > 0) {
            // Now initialize and fill an array with these free plots
            Plot[] freeplots = new Plot[free];
            for (int i=0, p=0; i < plotCount; i++) {
                if (plots[i].student == null) {
                    freeplots[p] = plots[i];
                    p++;
                }
            }

            // Randomly select a how many plots plots will get vacuous students
            int numVac = Random.Range(2, Mathf.Min(6, free+1));    // 2 to 5 vacuous students can come at most
            int[] vacPlotIdxs = new int[numVac];
            Plot[] vacPlots = new Plot[numVac];

            // Randomly select the indices of the freeplots to place vacuous students on
            for (int i = 0; i < numVac; i++) {
                bool dupes = true;
                while (dupes) {
                    vacPlotIdxs[i] = Random.Range(0, free);
                    dupes = false;

                    // Check if a duplicate index was generated
                    for (int j = 0; j < i; j++) {
                        if (vacPlotIdxs[j] == vacPlotIdxs[i]) {
                            dupes = true;
                        }
                    }
                }
            }

            // Get the plot objects and set to be occupied
            for (int i = 0; i < numVac; i++) {
                vacPlots[i] = freeplots[vacPlotIdxs[i]];
                vacPlots[i].student = vacuousStudent;
            }
            return vacPlots;
        }
        return null;
    }

    public void SpawnVacuousStudents() {
        // Assign vacuous students to plots
        Plot[] vac = AssignVacuousStudents();

        // Spawn the vacuous students on the plots if there is space
        if (vac != null) {
            foreach (Plot plot in vac) {
                Vector2 studentPosition = plot.transform.position;
                Instantiate(vacuousStudent, studentPosition, Quaternion.identity);
            }
        }
    }

    public static void GameOver() {
        Time.timeScale = 0;
        UIManager.ShowGameOverUI();
    }
}