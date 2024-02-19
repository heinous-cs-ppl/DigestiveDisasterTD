using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathfollow : MonoBehaviour
{   // index of the spawnpoint
    public int spawnIndex;

    // another class to get the spawnpoints
    private FindSpawnpoint spawnpointFind;

    // array of every spawnpoint
    private GameObject[] spawnpoints;

    // the spawnpoint of the enemy (From the Enemy Info script attached to the enemy prefab)
    private GameObject spawnpoint;

    // list with the gameobjects that form the path for the enemies
    private Transform[] path;

    // index of most recently visited gameobject
    private int currentNode = 0;

    // speed of the enemy (from the Enemy Info script attached to the enemy prefab)
    private float speed;

    // number of nodes in the path
    private int pathNodeCount;

    // position of the next node to be visited in the path
    Vector2 nextNodePos;
    // Start is called before the first frame update
    void Start()
    {   
        // get the index of the spawnpoint of the enemy that this script is attached to
        spawnIndex = GetComponent<EnemyInfo>().spawnPointIndex;
        // get the spawnpoint from the associated index
        getSpawnpoint();
        // initialize the path for the enemies to follow
        pathNodeCount = spawnpoint.transform.childCount;
        path = new Transform[pathNodeCount];
        
        for (int i = 0; i < pathNodeCount; i++)
        {
            path[i] = spawnpoint.transform.GetChild(i);
        }

        // get the speed of the enemies
        speed = GetComponent<EnemyInfo>().speed;
    }

    void MoveToNode(int nodeIndex)
    {
        // check if the index of the node is valid
        if (nodeIndex < 0 || nodeIndex >= path.Length)
        {
            Debug.LogError("pathfollow.cs: bad index");
            return;
        }
        nextNodePos = path[nodeIndex].position;
        transform.position = Vector2.MoveTowards(transform.position, nextNodePos, speed * Time.deltaTime);
    }
    // Update is called once per frame

    void getSpawnpoint()
    {
        // get a list of every spawnpoint
        spawnpointFind = new FindSpawnpoint();
        // GetSpawnpoints returns an array of Vector2 that hold the positions of every spawnpoint
        spawnpoints = spawnpointFind.GetSpawnpoints();
        
        // get the spawnpoint of the current enemy
        spawnpoint = spawnpoints[spawnIndex];
    }

    void Update()
    {
        if(currentNode == pathNodeCount)
        {
            // reached the end of the path
            return;
        }
        // move the enemy towards the current node in the path
        MoveToNode(currentNode);
        
        if(Vector2.Distance((Vector2)transform.position, nextNodePos) < 0.1f)
        {
            // if the enemy has reached the current node in the path 
            // update the current node to the next node
            currentNode++;
        }

    }

}
