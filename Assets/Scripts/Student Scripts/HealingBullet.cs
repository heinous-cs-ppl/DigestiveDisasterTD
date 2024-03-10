using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingBullet : Bullets
{  
    HealingTurret objectName;
    public GameObject healingAoe;
    private void Awake()
    {
        objectName = FindObjectOfType<HealingTurret>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        string collided = other.gameObject.name;
        string target = objectName.specificGameObject;
        // Will check if the collided target matches with the inteded target
        if (target == collided)
        {
            // Changes to explosion prefab
            Instantiate(healingAoe, transform.position, transform.rotation);

            // Destroy the bullet regardless of the collided object
            Destroy(gameObject);
        }
        
    }
}
