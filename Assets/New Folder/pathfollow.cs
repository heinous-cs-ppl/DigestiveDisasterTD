using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathfollow : MonoBehaviour
{   // spawn point of the enemy
    public GameObject spawnPoint;

    // list with the gameobjects that form the path for the enemies
    private Transform[] path;

    // index of most recently visited gameobject
    private int currentNode = 0;

    // speed of the enemy (default 5)
    public float speed = 5f;

    // number of nodes in the path
    private int pathNodeCount;

    // position of the next node to be visited in the path
    Vector2 nextNodePos;
    // Start is called before the first frame update
    void Start()
    {
        // initialize the path for the enemies to follow
        pathNodeCount = spawnPoint.transform.childCount;
        path = new Transform[pathNodeCount];
        for (int i = 0; i < pathNodeCount; i++)
        {
            path[i] = spawnPoint.transform.GetChild(i);
        }
    }

    void MoveToNode(int nodeIndex)
    {
        // check if the index of the node is valid
        if (nodeIndex < 0 || nodeIndex >= path.Length)
        {
            Debug.LogError("bad index");
            return;
        }
        nextNodePos = path[nodeIndex].position;
        transform.position = Vector2.MoveTowards(transform.position, nextNodePos, speed * Time.deltaTime);
    }
    // Update is called once per frame
    void Update()
    {
        if(currentNode == pathNodeCount)
        {
            // reached the end of the path
            return;
        }
        // move the enemy towards the current node in the path
        MoveToNode(currentNode);
        
        if(Vector2.Distance((Vector2)transform.position, nextNodePos) < 0.01f)
        {
            // if the enemy has reached the current node in the path 
            // update the current node to the next node
            currentNode++;
        }

    }

}
