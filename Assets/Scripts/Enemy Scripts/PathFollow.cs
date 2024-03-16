using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollow : MonoBehaviour {   
    // array of every spawnpoint
    private GameObject[] spawnpoints;

    // index of the spawnpoint
    private int spawnIndex;

    // the spawnpoint of the enemy (From the Enemy Info script attached to the enemy prefab)
    private GameObject spawnpoint;

    // list with the gameobjects that form the path for the enemies
    private Transform[] path;

    // number of nodes in the path
    private int pathNodeCount;

    // index of most recently visited gameobject
    private int currentNode = 0;

    // // most recently visited gameobject
    // private GameObject currNodeObj;

    // animator for the enemy (used to update the animation)
    public Animator anim;
    // position of the next node to be visited in the path
    Vector2 nextNodePos;

    // speed of the enemy (from the Enemy Info script attached to the enemy prefab)
    private float speed;
    private float originalSpeed;
    // For calculating slows
    private bool slowCheck = false;
    private float remainingTime;
    private Coroutine slowCoroutine;

    void MoveToNode(int nodeIndex) {
        // check if the index of the node is valid
        if (nodeIndex < 0 || nodeIndex >= path.Length) {
            Debug.LogError("pathfollow.cs: bad index");
            return;
        }
        nextNodePos = path[nodeIndex].position;

        // update the animation if necessary
        if (anim != null) {
            Vector2 dir = nextNodePos - (Vector2) transform.position;
            dir.x = Mathf.Round(dir.x);
            dir.y = Mathf.Round(dir.y);
            if (dir.x > 0) {
                anim.SetTrigger("SetMoveRight");
            } else if (dir.x < 0) {
                anim.SetTrigger("SetMoveLeft");
            } else if (dir.y < 0) {
                anim.SetTrigger("SetMoveDown");
            } else if (dir.y > 0) {
                anim.SetTrigger("SetMoveUp");
            } 
        }

        // transform.position refers to the Vector of the "current game object" - the enemy this script belongs to
        transform.position = Vector2.MoveTowards(transform.position, nextNodePos, speed * Time.deltaTime);
    }

    // Start is called before the first frame update
    void Start() {   
        // get the index of the spawnpoint of the enemy that this script is attached to
        spawnIndex = GetComponent<EnemyInfo>().spawnPointIndex;

        // get the spawnpoint from the associated index
        spawnpoints = LevelManager.instance.spawnObjs;
        spawnpoint = spawnpoints[spawnIndex];

        // initialize the path for the enemies to follow
        path = LevelManager.instance.NextPathLeg(spawnpoint, true);
        pathNodeCount = path.Length;

        // get the speed of the enemies
        speed = GetComponent<EnemyInfo>().speed;

        // Stores original speed
        originalSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentNode == pathNodeCount) {
            Transform lastNodeTransformObj = path[currentNode-1];   // The last node in path currently
            if (lastNodeTransformObj.childCount == 0) {
                // reached the end of the path
                LevelManager.instance.GameOver();
                Destroy(gameObject);
                return;
            }
            path = LevelManager.instance.NextPathLeg(lastNodeTransformObj.gameObject, false);  // This Transform was made from a GameObject, so we can access the GameObject it's made from
            pathNodeCount = path.Length;
            currentNode = 0;
        }

        // move the enemy towards the current node in the path
        MoveToNode(currentNode);
        
        if(Vector2.Distance((Vector2)transform.position, nextNodePos) < 0.01f) {
            // if the enemy has reached the current node in the path 
            // update the current node to the next node
            currentNode++;
        }
    }

    public void slowSpeed(float slow, float delay)
    {

        // Checks if the target has been slowed already
        if(slowCheck == false)
        {
            // Sets slowCheck to be true in order to prevent slows from stacking
            slowCheck = true;

            Debug.Log(delay);
            Debug.Log("Before: " + speed);
            // Sets the new speed of the slow 
            speed = speed*slow;
            Debug.Log("After: " + speed);
            
            // Stores the length of the delay duration into remaining
            remainingTime = delay;
            
            // Starts the timer
            slowCoroutine = StartCoroutine(NormalSpeedAfterDelay());
            Debug.Log("After delay: " + speed);

            
        } 
        else 
        {
            // Stops current timer and resets it if target is slowed again
            StopCoroutine(slowCoroutine);
            // Starts the timer
            slowCoroutine = StartCoroutine(NormalSpeedAfterDelay());
        }
    }

    IEnumerator NormalSpeedAfterDelay() 
    {
        // Will delay before reseting the speed back to its original speed
        yield return new WaitForSeconds(remainingTime);  
        speed = originalSpeed;
        Debug.Log("Back to original speed");   
        slowCheck = false;
    }
    
}
