using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    // Use this reference to access stuff from this class
    public static LevelManager instance;   

    // Number of starting paths or number of spawnpoints - each of these points is the parent of the rest of it's path
    public GameObject[] spawnObjs;      // given in object inspector

    [HideInInspector]
    public Transform[] spawnObjTransforms;

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
}
