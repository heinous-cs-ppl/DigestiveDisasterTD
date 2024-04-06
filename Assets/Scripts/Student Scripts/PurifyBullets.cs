using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PurifyBullet : Bullets
{
    public GameObject explosion;
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Take health from Enemy
        Instantiate(explosion, transform.position, transform.rotation);
        PlaySound.instance.SFX(sfx);

        // Destroy the bullet regardless of the collided object
        Destroy(gameObject);
    }
}
