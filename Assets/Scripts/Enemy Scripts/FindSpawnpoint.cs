// using UnityEngine;
// using System.Linq;

// public class FindSpawnpoint
// {
//     private Vector2[] spawnpointPositions;
//     private GameObject[] spawnpoints = GameObject.FindGameObjectsWithTag("Spawnpoint");

//     private Vector2[] FindSpawnpointTransforms()
//     {
//         // create array to hold the positions of the spawnpoints
//         Vector2[] positions = new Vector2[spawnpoints.Length];

//         // add the positions to the array
//         for (int i = 0; i < spawnpoints.Length; i++)
//         {
//             positions[i] = (Vector2) spawnpoints[i].transform.position;
//         }

//         return positions;
//     }

//     public Vector2[] GetSpawnpointPositions()
//     {   
//         // call this function from outside scripts to get the spawnpoint positions
//         spawnpoints = spawnpoints.OrderBy(obj => obj.name).ToArray();
//         spawnpointPositions = FindSpawnpointTransforms();
//         return spawnpointPositions;
//     }

//     public GameObject[] GetSpawnpoints()
//     {
//         // call this function form outside scripts to get the spawnpoint GameObjects
//         spawnpoints = spawnpoints.OrderBy(obj => obj.name).ToArray();
//         return spawnpoints;
//     }
// }
