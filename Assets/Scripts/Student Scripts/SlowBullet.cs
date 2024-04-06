using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowBullet : Bullets
{
    public GameObject explosion;

    [Header("Make sure to put it as the actual percent no in decimal form")]
    public float slow;
    public float duration;
    private void Start()
    {
        aoe aoeScript = explosion.GetComponent<aoe>();
        slow = slow/100;
        aoeScript.slowPercentage = slow;
        aoeScript.delay = duration;

    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Take health from Enemy
        Instantiate(explosion, transform.position, transform.rotation);
        PlaySound.instance.SFX(sfx);

        // Destroy the bullet regardless of the collided object
        Destroy(gameObject);

    }
}
