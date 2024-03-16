using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingBullet : Bullets
{  
    [HideInInspector] public string selectedTarget;
    public GameObject healingAoe;
    private void OnCollisionEnter2D(Collision2D other)
    {
        string collided = other.gameObject.name;
        Debug.Log(selectedTarget);
        // Will check if the collided target matches with the inteded target
        if (selectedTarget == collided)
        {
            // Changes to explosion prefab
            Instantiate(healingAoe, transform.position, transform.rotation);

            // Destroy the bullet regardless of the collided object
            Destroy(gameObject);
        }
        
    }
}
