using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingBullet : Bullets
{  
    HealingTurret objectName;
    private void Awake()
    {
        objectName = FindObjectOfType<HealingTurret>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        string collided = other.gameObject.name;
        string target = objectName.specificGameObject;

        Debug.Log(collided + ":" + target);

        // Will check if the collided target matches with the inteded target
        if (target == collided)
        {
            // Give health to allies
            other.gameObject.GetComponent<StudentInfo>().Heal(bulletDamage);
            // Destroy the bullet regardless of the collided object
            Destroy(gameObject);
        }
        
    }
}
