using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingBullet : Bullets
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Give health to allies
        other.gameObject.GetComponent<StudentInfo>().Heal(bulletDamage);
        Debug.Log("Healing");


        // Destroy the bullet regardless of the collided object
        Destroy(gameObject);
    }
}
